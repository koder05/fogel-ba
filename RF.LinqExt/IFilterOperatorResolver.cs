using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RF.LinqExt
{
    public interface IFilterOperatorResolver
    {
        Expression FindResolution(OperatorType op, Expression prop, Expression val);
        
        /// <summary>
        /// List of the constants that not will modified to sql params 
        /// </summary>
        /// <returns></returns>
        string[] GetStayConstants();
    }
}
