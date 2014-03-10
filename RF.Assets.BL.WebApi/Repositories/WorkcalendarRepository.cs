using System;
using System.Collections.Generic;
using System.Linq;

using RF.LinqExt;
using RF.BL;
using BLL = RF.BL.Model;
using WebApi.Svc;
using RF.BL.WebApi.DtoProxy;
using RF.BL.WebApi.Mapping;

namespace RF.Assets.BL.WebApi
{
    public class WorkcalendarRepository : IWorkcalendarRepository
    {
        private IFilterOperatorResolver opResolver = new WorkcalendarSqlOpResolver();
        private SortParameterCollection defaultSorting = new SortParameterCollection();
        private WebApiCtx _db;
        public WorkcalendarRepository(WebApiCtx db)
        {
            _db = db;
            defaultSorting.Add<WorkCalendar>("Date"); 
        }

        public int GetListCount(FilterParameterCollection filters)
        {
            lock (_db)
            {
                if (filters != null)
                    filters.OperatorActionResolver = opResolver;
                return _db.Holidays.AddFilters(filters).TotalCount();
            }
        }

        public IEnumerable<BLL.WorkCalendar> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy)
        {
            lock (_db)
            {
                orderBy.DefaultOrder = defaultSorting;
                int skip = pageIndex * pageSize;
                if (filters != null)
                    filters.OperatorActionResolver = opResolver;
                var list = _db.Holidays.AddFilters(filters).AddOrders(orderBy).Skip(skip).Take(pageSize);
                foreach (var m in list)
                    yield return ProxyActivator.CreateProxy<WorkCalendar, BLL.WorkCalendar>(m);
            }
        }

        public int GetIndexOf(BLL.WorkCalendar o, FilterParameterCollection filters, SortParameterCollection orderBy)
        {
            var condition = new FilterParameterCollection();
            condition.Add("Date", o.Date);
            if (filters != null)
                filters.OperatorActionResolver = opResolver;
            return _db.Holidays.AddFilters(filters).AddOrders(orderBy).IndexOf(condition);
        }

        public void Update(BLL.WorkCalendar o)
        {
            var proxy = o as IDtoProxy;
            if (proxy != null)
            {
                _db.UpdateObject(proxy.Dto);
                _db.SaveChanges(System.Data.Services.Client.SaveChangesOptions.ReplaceOnUpdate);
            }
        }

        public IQueryable<BLL.WorkCalendar> Context
        {
            get
            {
                return _db.Holidays as IQueryable<BLL.WorkCalendar>;
            }
        }
    }
}
