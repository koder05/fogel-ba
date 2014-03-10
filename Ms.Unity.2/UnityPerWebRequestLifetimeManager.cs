using System;
using System.Collections.Generic;
using System.Web;

using Microsoft.Practices.Unity;

namespace Ms.Unity._2
{
	public class UnityPerWebRequestLifetimeManager : LifetimeManager
	{
		private HttpContextBase _httpContext;

		public UnityPerWebRequestLifetimeManager()
			: this(new HttpContextWrapper(HttpContext.Current))
		{
		}

		public UnityPerWebRequestLifetimeManager(HttpContextBase httpContext)
		{
			_httpContext = httpContext;
            

            IHttpModule modul = _httpContext.ApplicationInstance.Modules.Get(typeof(UnityPerWebRequestLifetimeModule).Name);
            if (modul == null)
            {
                modul = new UnityPerWebRequestLifetimeModule(_httpContext);
                
                modul.Init(_httpContext.ApplicationInstance);
            }
		}

		private IDictionary<UnityPerWebRequestLifetimeManager, object> BackingStore
		{
			get
			{
				_httpContext = (HttpContext.Current != null) ? new HttpContextWrapper(HttpContext.Current) : _httpContext;
				return UnityPerWebRequestLifetimeModule.GetInstances(_httpContext);
			}
		}

		private object Value
		{
			get
			{
				IDictionary<UnityPerWebRequestLifetimeManager, object> backingStore = BackingStore;

				return backingStore.ContainsKey(this) ? backingStore[this] : null;
			}

			set
			{
				IDictionary<UnityPerWebRequestLifetimeManager, object> backingStore = BackingStore;

				if (backingStore.ContainsKey(this))
				{
					object oldValue = backingStore[this];

					if (!ReferenceEquals(value, oldValue))
					{
						IDisposable disposable = oldValue as IDisposable;

						if (disposable != null)
						{
							disposable.Dispose();
						}

						if (value == null)
						{
							backingStore.Remove(this);
						}
						else
						{
							backingStore[this] = value;
						}
					}
				}
				else
				{
					if (value != null)
					{
						backingStore.Add(this, value);
					}
				}
			}
		}

		public override object GetValue()
		{
			return Value;
		}

		public override void SetValue(object newValue)
		{
			Value = newValue;
		}

		public override void RemoveValue()
		{
			Value = null;
		}
	}
}
