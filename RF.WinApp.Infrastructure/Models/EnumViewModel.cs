using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.Common;

namespace RF.WinApp
{
    public class EnumViewModel<T> where T : struct
    {
        public int Index { get; set; }
        public T Value { get; set; }
        public string Name { get; set; }

        public IEnumerable<EnumViewModel<T>> GetList()
        {
            foreach (object val in Enum.GetValues(typeof(T)))
            {
                string enumName = Utils.GetEnumDescription<T>((T)val);
                if (string.IsNullOrEmpty(enumName))
                    enumName = Enum.GetName(typeof(T), val);
                yield return new EnumViewModel<T>() { Index = Convert.ToInt32((T)val), Name = enumName, Value = (T)val };
            }
        }
    }
}
