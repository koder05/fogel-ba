using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL.Model;
using RF.LinqExt;

namespace RF.BL
{
    public interface IGovernorRepository : ICUDRepository<Governor>
    {
        IQueryable Context { get; }
        Governor GetById(Guid id);
        int GetListCount(FilterParameterCollection filters);
        IEnumerable<Governor> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy);
        int GetIndexOf(Governor o, FilterParameterCollection filters, SortParameterCollection orderBy);
    }
}
