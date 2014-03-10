using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;

using RF.Common;
using RF.Common.DI;

namespace RF.WebApp
{
    class ScopeContainer : IDependencyScope
    {
        protected IContainerWrapper container;

        public ScopeContainer(IContainerWrapper container)
        {
            Args.IsNotNull(container, "container");
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            if (typeof(IHttpController).IsAssignableFrom(serviceType))
            {
                try
                {
                    return (object)this.container.Resolve<IHttpController>(serviceType);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(String.Format("Error resolving api controller {0}", serviceType.Name), ex);
                }
            }
            
            if (container.IsRegistered(serviceType))
            {
                return container.Resolve<object>(serviceType);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (container.IsRegistered(serviceType))
            {
                return container.ResolveAll(serviceType);
            }
            else
            {
                return new List<object>();
            }
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }

    class DependencyResolver : ScopeContainer, IDependencyResolver
    {
        public DependencyResolver(IContainerWrapper container)
            : base(container)
        {
        }

        public IDependencyScope BeginScope()
        {
            return new ScopeContainer(container.Bud());
        }
    }
}