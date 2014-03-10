using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL;
using RF.BL.Model;
using RF.LinqExt;

namespace RF.WinApp
{
    public class WorkcalendarDataViewProvider : IDataView
    {
        private IWorkcalendarRepository _rep;

        public WorkcalendarDataViewProvider(IWorkcalendarRepository rep)
        {
            _rep = rep;
        }

        public Type ModelType { get { return typeof(WorkCalendar); } }

        public object ActivateEmptyModel()
        {
            throw new NotImplementedException();
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
            return _rep.GetIndexOf((WorkCalendar)o, filters, orderBy);
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
            _rep.Update(o as WorkCalendar);
        }
    }
}
