using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RF.LinqExt
{
	public interface IFilterSortPropResolver
	{
		IEnumerable<LambdaExpression> FindResolution(Type sourceType, Type destType, ListControlActionType actionType, string memberName);
	}
}
