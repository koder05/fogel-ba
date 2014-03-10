using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace RF.LinqExt
{
    public class SortParameterCollection : Collection<SortParameter>
    {
        public SortParameterCollection()
            : base()
        {
        }

        public SortParameterCollection(Type type, string columnName, ListSortDirection dir)
            : base()
        {
            Add(new SortParameter(type, columnName, dir));
        }

        public SortParameterCollection(IEnumerable<SortParameter> list)
            : base(list.ToList())
        {
        }

        public void Add<T>(string columnName, ListSortDirection dir) where T : class, new()
        {
            this.Add(typeof(T), columnName, dir);
        }

        public void Add<T>(string columnName) where T : class, new()
        {
            this.Add<T>(columnName, ListSortDirection.Ascending);
        }

        /// <summary>
        /// Use for determenistic ordering by non-unique fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName"></param>
        public SortParameterCollection DefaultOrder { get; set; }

        public IFilterSortPropResolver PropertyNameResolver { get; set; }

        public void Add(Type modelType, string columnName, ListSortDirection dir)
        {
            Remove(modelType, columnName);
            this.Add(new SortParameter(modelType, columnName, dir));
        }

        public void Remove<T>(string columnName) where T : class, new()
        {
            this.Remove(typeof(T), columnName);
        }

        public void Remove(Type modelType, string columnName)
        {
            foreach (var par in this.Where(p => p.Type == modelType && p.ColumnName == columnName).ToList())
                this.Remove(par);
        }
    }
}
