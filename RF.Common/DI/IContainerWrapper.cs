using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.DI
{
	public interface IContainerWrapper : IDisposable
	{
		T Resolve<T>(Type type);
		T Resolve<T>();
        IEnumerable<object> ResolveAll(Type type);
        IContainerWrapper Bud();
        bool IsRegistered(Type type);
	}
}
