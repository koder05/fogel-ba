using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RF.Common
{
	[Serializable]
	public class ComparableCollection<T> : ReadOnlyCollection<T>
	{
		public ComparableCollection(IList<T> list)
			: base(list)
		{
		}

		public override bool Equals(object obj)
		{
			ComparableCollection<T> col = obj as ComparableCollection<T>;
			if (obj == null)
				return false;

			return Utils.ListsAreEqual<T>(this, col);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}