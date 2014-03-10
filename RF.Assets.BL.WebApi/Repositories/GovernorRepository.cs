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
    public class GovernorRepository : IGovernorRepository
    {
        private SortParameterCollection defaultSorting = new SortParameterCollection();
        private WebApiCtx _db;
        private Model2DtoMapper _mapper;
        public GovernorRepository(WebApiCtx db, Model2DtoMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            defaultSorting.Add<BLL.Governor>("Id");
        }

        public int GetListCount(FilterParameterCollection filters)
        {
            lock (_db)
            {
                return _db.Governors.AddFilters(filters).TotalCount();
            }
        }

        public IEnumerable<BLL.Governor> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy)
        {
            lock (_db)
            {
                orderBy.DefaultOrder = defaultSorting;
                int skip = pageIndex * pageSize;
                var list = _db.Governors.AddFilters(filters).AddOrders(orderBy).Skip(skip).Take(pageSize);
                foreach (var m in list)
                    yield return ProxyActivator.CreateProxy<Governor, BLL.Governor>(m);
            }
        }

        public int GetIndexOf(BLL.Governor o, FilterParameterCollection filters, SortParameterCollection orderBy)
        {
            return _db.Governors.AddFilters(filters).AddOrders(orderBy).IndexOf(o.Id);
        }

        public BLL.Governor GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public object Create(BLL.Governor o)
        {
            var dto = _mapper.Map<BLL.Governor, Governor>(o);
            _db.AddToGovernors(dto);
            _db.SaveChanges();
            return o;
        }

        public void Delete(BLL.Governor o)
        {
            var proxy = o  as IDtoProxy;
            if (proxy != null)
            {
                _db.DeleteObject(proxy.Dto);
                _db.SaveChanges();
            }
        }

        public void Update(BLL.Governor o)
        {
            var proxy = o as IDtoProxy;
            if (proxy != null)
            {
                _db.UpdateObject(proxy.Dto);
                _db.SaveChanges(System.Data.Services.Client.SaveChangesOptions.ReplaceOnUpdate);
            }
        }

        public IQueryable Context
        {
            get
            {
                return _db.Governors as IQueryable;
            }
        }
    }
}
