using System;
using System.Collections.Generic;

using RF.Common;
using RF.Common.DI;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Ms.Unity._2
{
	public class NonUnityContainer : IContainerWrapper
	{
		private UnityPerWebRequestLifetimeManager _perReqMan;
		public NonUnityContainer()
		{
			_perReqMan = new UnityPerWebRequestLifetimeManager();
		}
		
		
		#region IContainerWrapper Members

		public T Resolve<T>(Type type)
		{
			throw new NotImplementedException();
		}

		public T Resolve<T>()
		{
			object ret = _perReqMan.GetValue();
			if (ret == null)
			{
				ret = Activator.CreateInstance<T>();  
				_perReqMan.SetValue(ret);
			}
			return (T)ret;
		}

        public IEnumerable<object> ResolveAll(Type type)
        {
            throw new NotImplementedException();
        }

        public IContainerWrapper Bud()
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type type)
        {
            throw new NotImplementedException();
        }

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			
		}

		#endregion
	}
}
