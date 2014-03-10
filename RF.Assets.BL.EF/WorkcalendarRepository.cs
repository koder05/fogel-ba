using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Objects.SqlClient;
using System.Text;

using RF.BL;
using RF.BL.Model;
using RF.Assets.BL;
using RF.BL.EF.Repositories;
using RF.LinqExt;
using EF;
using EF.Sql;

namespace RF.BL.EF
{
    public class WorkcalendarRepository : IWorkcalendarRepository
    {
        private IFilterOperatorResolver opResolver = new WorkcalendarSqlOpResolver();
        private SortParameterCollection defaultSorting = new SortParameterCollection();
        private AssetsEFCtx _db;
        public WorkcalendarRepository(AssetsEFCtx db)
        {
            _db = db;
            defaultSorting.Add<WorkCalendar>("Date"); 
        }

        public int GetListCount(FilterParameterCollection filters)
        {
            lock (_db)
            {
                return _db.Holidays.Filtering(filters, opResolver).Count();
            }
        }

        public IEnumerable<WorkCalendar> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy)
        {
            lock (_db)
            {
                orderBy.DefaultOrder = defaultSorting;
                var list = _db.Holidays.Filtering(filters, opResolver).Sorting(orderBy).Paging(pageIndex, pageSize);
                foreach (var m in list)
                    yield return m;
            }
        }

        public int GetIndexOf(WorkCalendar o, FilterParameterCollection filters, SortParameterCollection orderBy)
        {
            orderBy.DefaultOrder = defaultSorting;
            if (filters != null) filters.OperatorActionResolver = opResolver;
            return _db.Holidays.GetIndexOf(filters, orderBy, poco => poco.Date == o.Date);
        }


        public void Update(WorkCalendar o)
        {
            _db.Database.ExecuteSqlCommand("exec dbo.UpdateWorkCalendar @iswd, @d, @comment"
                    , new System.Data.SqlClient.SqlParameter("@iswd", o.IsWorkingDay)
                    , new System.Data.SqlClient.SqlParameter("@d", o.Date)
                    , new System.Data.SqlClient.SqlParameter("@comment", o.Comment));
        }

        public IQueryable<WorkCalendar> Context
        {
            get
            {
                return _db.Holidays;
            }
        }
    }
}
