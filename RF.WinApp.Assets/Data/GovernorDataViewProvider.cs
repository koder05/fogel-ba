using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL;
using RF.BL.Model;
using RF.LinqExt;

namespace RF.WinApp
{
    public class GovernorDataViewProvider : IDataView
    {
        private IGovernorRepository _rep;

        public GovernorDataViewProvider(IGovernorRepository rep)
        {
            _rep = rep;
        }

        public Type ModelType { get { return typeof(Governor); } }

        public object ActivateEmptyModel() 
        {
            Company c = new Company();
            Governor g = new Governor();
            g.CompanyId = c.Id;
            g.Company = c;
            return g;
        }

        public int GetListCount(FilterParameterCollection filters)
        {
            return _rep.GetListCount(filters);
        }

        public IEnumerable<object> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy)
        {
            return _rep.GetList(filters, pageIndex, pageSize, orderBy).Cast<object>().ToList(); 
        }

        public int GetIndexOf(object o, FilterParameterCollection filters, SortParameterCollection orderBy)
        {
            return _rep.GetIndexOf((Governor)o, filters, orderBy);
        }

        public void Create(object o)
        {
            _rep.Create(o as Governor);
        }

        public void Delete(IEnumerable<object> pool)
        {
            foreach (var o in pool)
            _rep.Delete(o as Governor);
        }

        public void Update(object o)
        {
            _rep.Update(o as Governor);
        }
    }
}
