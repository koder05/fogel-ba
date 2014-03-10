using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RF.LinqExt
{
	public class FilterParameterCollection : FilterParameterCollectionBase
	{
        private Type _modelType = null;
        public FilterParameterCollection(Type modelType)
        {
            _modelType = modelType;
        }

        public FilterParameterCollection()
        {
        }
        
        public override Expression<Func<T, bool>> GetLinqCondition<T>(IFilterSortPropResolver propResolver, IFilterOperatorResolver opResolver)
		{
			Expression body = null;
			ParameterExpression mainObject = Expression.Parameter(typeof(T), "obj");

			foreach (FilterParameter p in this)
			{
				if (p.Value == null)
					continue;

				Expression value = Expression.Constant(p.Value, p.Value.GetType());

				Expression propVal = mainObject;
				Type objType = typeof(T);

				IEnumerable<LambdaExpression> exprList = null;
				if (propResolver != null)
				{
                    exprList = propResolver.FindResolution(_modelType ?? objType, objType, ListControlActionType.Filtering, p.ColumnName);
				}

				if (exprList != null && exprList.Count() > 0)
				{
					var modifier = new MainObjectLinkModifier(mainObject);
					propVal = modifier.Visit(exprList.ElementAt(0).Body);
				}
				else
				{
					string[] propTree = p.ColumnName.Split('.');
					foreach (string prop in propTree)
					{
						PropertyInfo pi = objType.GetProperty(prop, BindingFlags.Instance | BindingFlags.Public);
						if (pi != null)
						{
							propVal = Expression.Property(propVal, pi);
							objType = pi.PropertyType;
						}
					}
				}

				Expression boolOperation = null;
                if (opResolver != null)
                {
                    boolOperation = opResolver.FindResolution(p.Operator, propVal, value);
                }

				if (boolOperation != null)
				{
					if (body == null)
					{
						body = boolOperation;
					}
					else
					{
						body = Expression.And(body, boolOperation);
					}
				}
			}

			if (body == null)
			{
				body = Expression.Equal(Expression.Constant(1), Expression.Constant(1));
			}
			else
			{
				var modifier = new ConstantModifier();
                if (opResolver != null)
                {
                    modifier.StayConstants = opResolver.GetStayConstants();
                }
				body = modifier.Visit(body);
			}

			Expression<Func<T, bool>> res = Expression.Lambda<Func<T, bool>>(body, mainObject);

			return res;
		}
	}

	public abstract class FilterParameterCollectionBase : Collection<FilterParameter>
	{
		/// <summary>
		/// Добавляет фильтрацию по определённой колонку в коллекцию
		/// </summary>
		/// <param name="columnName">Наименование колонки</param>
		/// <param name="value">Значение</param>
		/// <param name="op">Оператор</param>
		public void Add(string columnName, object value, OperatorType op)
		{
			this.Add(new FilterParameter(columnName, value, op));
		}

		public void Add(string columnName, object value)
		{
			this.Add(new FilterParameter(columnName, value, OperatorType.Equals));
		}

		public void Add(string columnName, object value, OperatorType op, string orGroup)
		{
			this.Add(new FilterParameter(columnName, value, op, orGroup));
		}

		public void AddCollection(FilterParameterCollection fc)
		{
			foreach (FilterParameter fp in fc)
			{
				this.Add(fp);
			}
		}

		public FilterParameter Find(string columnName, OperatorType ot)
		{
			return FindCore(columnName, ot, false);
		}

		public FilterParameter FindWithException(string columnName, OperatorType ot)
		{
			return FindCore(columnName, ot, true);
		}

		public FilterParameter Find(string columnName)
		{
			return FindCore(columnName, null, false);
		}

		public FilterParameter FindWithException(string columnName)
		{
			return FindCore(columnName, null, true);
		}

        public IFilterSortPropResolver PropertyNameResolver { get; set; }

        private IFilterOperatorResolver _opResolver = new DefaultFilterOperatorResolver();
        public IFilterOperatorResolver OperatorActionResolver 
        {
            get
            {
                return _opResolver;
            }
            set
            {
                if (value != null)
                    _opResolver = value;
            }
        }

		private FilterParameter FindCore(string columnName, OperatorType? ot, bool throwException)
		{
			foreach (FilterParameter fp in this)
				if (StringComparer.InvariantCultureIgnoreCase.Equals(fp.ColumnName, columnName) && (ot == null || fp.Operator == ot))
					return fp;

			if (throwException)
			{
				string typeDiagn = (ot != null) ? string.Format(" типа {0}", ot) : string.Empty;
				string errorMes = string.Format("FilterParameterCollection не содержит параметр \"{0}\"{1}.", columnName, typeDiagn);
				throw new ArgumentException(errorMes);
			}
			else
				return null;
		}

        public abstract Expression<Func<T, bool>> GetLinqCondition<T>(IFilterSortPropResolver propResolver, IFilterOperatorResolver opResolver) where T : class, new();
	}

	public class MainObjectLinkModifier : ExpressionVisitor
	{
		private ParameterExpression _mainObject {get; set; }

        public MainObjectLinkModifier(ParameterExpression mainObject)
		{
			_mainObject = mainObject;
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			if (node.NodeType == ExpressionType.Parameter && node.Type.IsClass && node.Type == _mainObject.Type && node != _mainObject)
			{
				return _mainObject;
			}
			
			return node;
		} 
	}

	/// <summary>
	/// work around to adjust parameterized expression vs constants
	/// </summary>
	public class ConstantModifier : ExpressionVisitor
	{
        public string[] StayConstants { get; set; }
        
        private class ParamValue
		{
			public Type type { get; set; }
			public object value { get; set; }
		}

 		protected override Expression VisitConstant(ConstantExpression node)
		{
			if (node.NodeType == ExpressionType.Constant && (node.Type.IsValueType || node.Type == typeof(string)))
			{
                if (StayConstants != null && StayConstants.Contains(node.Value.ToString()))
                    return node;
                
                var p = new ParamValue() { value = node.Value, type = node.Type };
				var expr = Expression.Convert(Expression.Property(Expression.Constant(p), "value"), p.type);
				return expr;
			}
			else return node;
		}

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }
	}

    public class DefaultFilterOperatorResolver : IFilterOperatorResolver
    {
        public virtual Expression FindResolution(OperatorType op, Expression prop, Expression val)
        {
            Expression boolOperation = null;
            switch (op)
            {
                case OperatorType.Equals:
                    boolOperation = Expression.Equal(prop, val);
                    break;
                case OperatorType.NotEquals:
                    boolOperation = Expression.NotEqual(prop, val);
                    break;
                case OperatorType.Like:
                    boolOperation = Expression.Call(prop, typeof(string).GetMethod("Contains"), val);
                    //boolOperation = Expression.Call(typeof(System.Data.Objects.SqlClient.SqlLike).GetMethod("Contains"), value); 
                    break;
                case OperatorType.LessOrEquals:
                    boolOperation = Expression.LessThanOrEqual(prop, val);
                    break;
                case OperatorType.MoreOrEquals:
                    boolOperation = Expression.GreaterThanOrEqual(prop, val);
                    break;
                case OperatorType.LessThan:
                    boolOperation = Expression.LessThan(prop, val);
                    break;
                case OperatorType.MoreThan:
                    boolOperation = Expression.GreaterThan(prop, val);
                    break;
            }
            return boolOperation;
        }

        public virtual string[] GetStayConstants()
        {
            return null;
        }
    }
}