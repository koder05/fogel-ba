using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using EF.Poco;
using RF.BL.Model;

namespace RF.BL.EF.PocoProxy
{
	public static class ProxyActivator
	{
		public static TModel CreateProxy<TPoco, TModel>(TPoco poco)
			where TPoco : BasePoco
			where TModel : BaseModel
		{
            Type proxyType = Type.GetType(typeof(ProxyActivator).Namespace + "." + typeof(TModel).Name + "Proxy");
			return (TModel)Activator.CreateInstance(proxyType, poco);
		}
	}
}
