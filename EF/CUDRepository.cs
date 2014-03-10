using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using BLModel;
using Model.ListControl;
using EFCF.PocoProxy;

namespace EF
{
	public class CUDRepository
	{
		protected EFContext db;
		protected IFilterSortPropResolver propMapper;
		private Model2PocoMapper mapper;

		public CUDRepository(EFContext _context, Model2PocoMapper _mapper, IFilterSortPropResolver _propMapper)
		{
			db = _context;
			mapper = _mapper;
			propMapper = _propMapper;
		}

		internal void CUD<TModel, TPoco>(TModel m, EntityState state)
			where TModel : BaseModel
			where TPoco : BasePoco, new()
		{
			if (m is IPocoProxy == false)
			{
				ObjectStateEntry entry = db.FindEntityById<TPoco>(m.Id);

				if (entry != null )
				{
					entry.ChangeState(state); 
					mapper.Map<TModel, TPoco>(m, (TPoco)entry.Entity);
				}
				else
				{
					TPoco poco = mapper.Map<TModel, TPoco>(m);
					db.Entry<TPoco>(poco).State = state;
				}
			}

			db.SaveChanges();
		}
	}
}
