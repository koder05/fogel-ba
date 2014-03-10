using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RF.LinqExt
{
	internal class PropMap
	{
		internal Type SourceType {get; private set;}
		internal Type DestinationType { get; private set; }
		internal ListControlActionType ListControlAction { get; private set; }

		internal PropMap(Type srcType, Type destType, ListControlActionType actionType)
		{
			SourceType = srcType;
			DestinationType = destType;
			ListControlAction = actionType; 
		}

		internal Dictionary<string, List<LambdaExpression>> Members = new Dictionary<string, List<LambdaExpression>>();

		public override bool Equals(object obj)
		{
			PropMap map = obj as PropMap;

			if (map != null)
				return this.SourceType.Equals(map.SourceType) && this.DestinationType.Equals(map.DestinationType) && this.ListControlAction.Equals(map.ListControlAction);  

			return false;
		}

		public override int GetHashCode()
		{
			return this.SourceType.GetHashCode() ^ this.DestinationType.GetHashCode() ^ this.ListControlAction.GetHashCode();
		}
	}
}
