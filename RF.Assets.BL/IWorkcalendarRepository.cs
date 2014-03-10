using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL.Model;
using RF.LinqExt;

namespace RF.BL
{
    public interface IWorkcalendarRepository
    {
        IQueryable<WorkCalendar> Context { get; }
        int GetListCount(FilterParameterCollection filters);
        IEnumerable<WorkCalendar> GetList(FilterParameterCollection filters, int startRowIndex, int maximumRows, SortParameterCollection orderBy);
        int GetIndexOf(WorkCalendar o, FilterParameterCollection filters, SortParameterCollection orderBy);
        void Update(WorkCalendar o);
    }
}
