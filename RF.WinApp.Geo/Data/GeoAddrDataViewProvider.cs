using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.LinqExt;
using RF.Geo.BL;
using RF.Geo.Parsers;

namespace RF.WinApp
{
    public class GeoAddrDataViewProvider : IDataView
    {
        public Type ModelType
        {
            get { return typeof(Addr); }
        }

        public object ActivateEmptyModel()
        {
            throw new NotImplementedException();
        }

        public int GetListCount(FilterParameterCollection filters)
        {
            return Addr.GetListCount(filters);
        }

        public int GetIndexOf(object o, FilterParameterCollection filters, SortParameterCollection orderBy)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy)
        {
            return Addr.GetList(filters, pageIndex, pageSize);
        }

        public void Create(object o)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<object> pool)
        {
            throw new NotImplementedException();
        }

        public void Update(object o)
        {
            Addr.Save(o as Addr);
        }
    }
}
