using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.Common.DI;

namespace RF.WinApp
{
    public class DataViewProviderFactory
    {
        public IDataView GetInstance(Type dwType)
        {
            if (dwType == null)
                throw new ArgumentNullException("dwType");

            if (!typeof(IDataView).IsAssignableFrom(dwType))
                throw new ArgumentException(string.Format("Type requested is not a data view provider: {0}", dwType.Name), "dwType");
            try
            {
                return IoC.Resolve<IDataView>(dwType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format("Error resolving data view provider {0}", dwType.Name), ex);
            }
        }
    }
}
