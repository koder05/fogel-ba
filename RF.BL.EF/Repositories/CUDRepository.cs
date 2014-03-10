using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using EF;
using EF.Poco;
using RF.BL.Model;
using RF.BL.EF.PocoProxy;
using RF.BL.EF.Mapping;  

namespace RF.BL.EF.Repositories
{
    public class CUDRepository<T> : ICUDRepository<T> where T : BaseModel
	{
		protected EFContext db;
		//protected IFilterSortPropResolver propMapper;
		private Model2PocoMapper mapper;

		public CUDRepository(EFContext _context, Model2PocoMapper _mapper/*, IFilterSortPropResolver _propMapper*/)
		{
			db = _context;
			mapper = _mapper;
			//propMapper = _propMapper;
		}

        public CUDRepository(EFContext _context)
        {
            db = _context;
            mapper = new Model2PocoMapper();
            //propMapper = _propMapper;
        }

        /// <summary>
        /// CUD for biz logic model without EF link
        /// </summary>
        internal void CUD<TModel, TPoco>(TModel m, EntityState state)
			where TModel : BaseModel
			where TPoco : BasePoco, new()
		{
            IPocoProxy proxy = m as IPocoProxy;
            if (proxy == null)
            {
                ObjectStateEntry entry = db.FindEntityById<TPoco>(m.Id);
                TPoco poco;

                if (entry != null)
                {
                    entry.ChangeState(state);
                    poco = (TPoco)entry.Entity;
                    mapper.Map<TModel, TPoco>(m, poco);
                }
                else
                {
                    poco = mapper.Map<TModel, TPoco>(m);
                    db.Entry<TPoco>(poco).State = state;
                }
            }
            else if (proxy.Poco.Id != Guid.Empty)
            {
                ObjectStateEntry entry = db.FindEntityById<TPoco>(proxy.Poco.Id);
                if (entry == null)
                {
                    db.Entry<TPoco>((TPoco)proxy.Poco).State = state;
                }
            }

			db.SaveChanges();
		}

        internal void CUD<TPoco>(T m, EntityState state) where TPoco : BasePoco, new()
        {
            CUD<T, TPoco>(m, state);
        }

        /// <summary>
        /// CUD for biz logic model it also POCO
        /// </summary>
        internal void CUD(T m, EntityState state)
        {
            db.Entry<T>(m).State = state;
            db.SaveChanges();
        }

        public virtual object Create(T o)
        {
            CUD(o, EntityState.Added);
            return o.Id;
        }

        public virtual void Delete(T o)
        {
            CUD(o, EntityState.Deleted);
        }

        public virtual void Update(T o)
        {
            CUD(o, EntityState.Modified);
        }
    }
}
