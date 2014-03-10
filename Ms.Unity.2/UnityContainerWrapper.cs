using System;
using System.Collections.Generic;

using RF.Common;
using RF.Common.DI;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Ms.Unity._2
{
	public class UnityContainerWrapper : IContainerWrapper
	{
		public UnityContainerWrapper()
			: this(new UnityContainer())
		{
			UnityConfigurationSection configuration = (UnityConfigurationSection)System.Configuration.ConfigurationManager.GetSection("unity");
			if (configuration != null)
				configuration.Configure(_container);
		}

		public UnityContainerWrapper(IUnityContainer container)
		{
			Args.IsNotNull(container, "container");
			_container = container;
		}

        #region IContainerWrapper implementation

        public T Resolve<T>(Type type)
		{
			Args.IsNotNull(type, "type");

			return (T)_container.Resolve(type);
		}

		public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}

        public IEnumerable<object> ResolveAll(Type type)
        {
            Args.IsNotNull(type, "type");
            
            return _container.ResolveAll(type);
        }

        public IContainerWrapper Bud()
        {
            return new UnityContainerWrapper(_container.CreateChildContainer()); 
        }

        public bool IsRegistered(Type type)
        {
            return _container.IsRegistered(type); 
        }

		#endregion

		#region IDisposible implementation

		~UnityContainerWrapper()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				_container.Dispose();
			}
		}

		#endregion

		private readonly IUnityContainer _container;
	}
}
