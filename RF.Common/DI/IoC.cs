using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.DI
{
	public static class IoC
	{
		public static void InitializeWith(IContainerWrapper resolver)
		{
			Args.IsNotNull(resolver, "resolver");

			_resolver = resolver;
		}

		public static T Resolve<T>(Type type)
		{
			Args.IsNotNull(type, "type");

			return _resolver.Resolve<T>(type);
		}

		public static T Resolve<T>()
		{
			return _resolver.Resolve<T>();
		}

        public static IEnumerable<T> ResolveAll<T>()
        {
            return _resolver.ResolveAll(typeof(T)).Cast<T>(); 
        }

		public static void Reset()
		{
			if (_resolver != null)
			{
				_resolver.Dispose();
			}
		}

		private static IContainerWrapper _resolver;
	}
}
