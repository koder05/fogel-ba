using System;
using System.Collections.Generic;
using System.Linq;

using RF.BL;
using RF.BL.Model;
using RF.BL.EF.Repositories;
using RF.LinqExt;
using EF;
using EF.Sql;

namespace RF.BL.EF
{
    public class GovernorRepository : CUDRepository<Governor>, IGovernorRepository
    {
        private IFilterOperatorResolver opResolver = new SqlOperatorResolver();
        private SortParameterCollection defaultSorting = new SortParameterCollection();
        private AssetsEFCtx _db;
        public GovernorRepository(AssetsEFCtx db)
            : base(db, null)
        {
            _db = db;
            defaultSorting.Add<Governor>("Id"); 
        }

        public int GetListCount(FilterParameterCollection filters)
        {
            lock (_db)
            {
                return _db.Governors.Filtering(filters, opResolver).Count();
            }
        }

        public IEnumerable<Governor> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy)
        {
            lock (_db)
            {
                orderBy.DefaultOrder = defaultSorting;
                var list = _db.Governors.Filtering(filters, opResolver).Sorting(orderBy).Paging(pageIndex, pageSize);
                foreach (var m in list)
                    yield return m;
            }
        }

        public int GetIndexOf(Governor o, FilterParameterCollection filters, SortParameterCollection orderBy)
        {
            orderBy.DefaultOrder = defaultSorting;
            if (filters != null) filters.OperatorActionResolver = opResolver;
            return _db.Governors.GetIndexOf(filters, orderBy, poco => poco.Id == o.Id);
        }

        public Governor GetById(Guid id)
        {
            return _db.Governors.FirstOrDefault(poco => poco.Id == id);
        }

        public IQueryable Context
        {
            get 
            {
                return _db.Governors as IQueryable; 
            }
        }

        public override void Update(Governor o)
        {
            _db.Entry<Company>(o.Company).State = System.Data.EntityState.Modified;
            base.Update(o);
        }

        public override void Delete(Governor o)
        {
            _db.Entry<Company>(o.Company).State = System.Data.EntityState.Deleted;
            base.Delete(o);
        }
    }
}
