using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace RF.LinqExt
{
	public class PropMapExpr<TSource, TDest>
		where TSource : class, new()
		where TDest : class, new()
	{
		private PropMap _map;

		internal PropMapExpr(PropMap map)
		{
			_map = map;
		}

		public PropMapExpr<TSource, TDest> ForMember<TValue>(Expression<Func<TSource, object>> sourceMember, Expression<Func<TDest, TValue>> destMember)
		{
			string key = GetMemberNameFromExpression(sourceMember);

			if (!string.IsNullOrEmpty(key))
			{
				if (!this._map.Members.ContainsKey(key))
					this._map.Members.Add(key, new List<LambdaExpression>());

				this._map.Members[key].Add(destMember);
			}

			return this;
		}

		private string GetMemberNameFromExpression(LambdaExpression lambdaExpression)
		{
			StringBuilder sb = new StringBuilder();
			Expression operand = lambdaExpression;
			while (true)
			{
				MemberExpression expression2;
				ExpressionType nodeType = operand.NodeType;
				if (nodeType != ExpressionType.Convert)
				{
					if (nodeType == ExpressionType.Lambda)
					{
						operand = ((LambdaExpression)operand).Body;
					}
					else if (nodeType == ExpressionType.MemberAccess)
					{
						expression2 = (MemberExpression)operand;
						sb.Insert(0, string.Format(".{0}", expression2.Member.Name));

						if (expression2.Expression.NodeType == ExpressionType.MemberAccess)
						{
							operand = expression2.Expression;
						}
						else if ((expression2.Expression.NodeType == ExpressionType.Parameter) || (expression2.Expression.NodeType == ExpressionType.Convert))
						{
							break;
						}
						else
						{
							throw new ArgumentException(string.Format("Expression '{0}' must resolve to top-level member.", lambdaExpression), "lambdaExpression");
						}
					}
				}
				else
				{
					operand = ((UnaryExpression)operand).Operand;
				}
			}

			return sb.ToString().TrimStart('.');
		}
	}
}
