using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL.Model;
using Svc = WebApi.Svc;

namespace RF.BL.WebApi.DtoProxy
{
    public class WorkCalendarProxy : WorkCalendar, IDtoProxy
    {
        public WorkCalendarProxy(Svc.WorkCalendar dto)
            : base()
        {
            if (dto == null)
                throw new InvalidOperationException("WorkCalendar dto");

            this.Id = dto.Id;
            this.Comment = dto.Comment;
            this.Date = dto.Date;
            this.IsWorkingDay = dto.IsWorkingDay;

            this._dto = dto;
            this.PropertyChanged += ProxyActivator.ReflectChangedProperty;
        }

        private Svc.WorkCalendar _dto;
        public object Dto { get { return _dto; } }
    }
}
