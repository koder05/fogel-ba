using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace RF.Common
{
	/// <summary>
	/// Environment для отбора одного енума посредством другого, через внедрение второго в атрибуты первого
	/// </summary>
	/// <typeparam name="TS"></typeparam>
	public interface IEnumSelector<TS> where TS : struct
	{
		TS Selector { get; }
	}
	
	public class EnumSelector
	{
		public static List<TR> GetTypes<TR, TS, TA>(TS selector)
			where TR : struct
			where TS : struct
			where TA : Attribute, IEnumSelector<TS>
		{
			Type t = typeof(TR);
			if (t.IsEnum == false)
				throw new ArgumentException("Selecting type (TR) must be enum type.");

			if (typeof(TS).IsEnum == false)
				throw new ArgumentException("Selector (TS) must be enum type.");

			List<TR> typeList = new List<TR>();
			foreach (string name in Enum.GetNames(t))
			{
				TA attr = (TA)Attribute.GetCustomAttribute(t.GetField(name), typeof(TA));
				if (attr != null && attr.Selector.Equals(selector))
					typeList.Add((TR)Enum.Parse(t, name));
			}
			return typeList;
		}

		public static TS GetSelector<TR, TS, TA>(TR type)
			where TR : struct
			where TS : struct
			where TA : Attribute, IEnumSelector<TS>
		{
			Type t = typeof(TR);
			if (t.IsEnum == false)
				throw new ArgumentException("Selecting type (TR) must be enum type.");

			if (typeof(TS).IsEnum == false)
				throw new ArgumentException("Selector (TS) must be enum type.");

			TS ret = default(TS);
			string name = Enum.GetName(t, type);

			if (name == null)
				throw new ArgumentException("Invalid enum.");

			FieldInfo fi = t.GetField(name);

			TA attr = (TA)Attribute.GetCustomAttribute(fi, (typeof(TA)));
			if (attr != null)
				ret = attr.Selector;

			return ret;
		}
	}
}
