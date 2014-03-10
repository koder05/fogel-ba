using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

using RF.BL.Model;

namespace RF.BL.WebApi.DtoProxy
{
    public static class ProxyActivator
    {
        public static TModel CreateProxy<TDto, TModel>(TDto dto)
            where TDto : class, new()
            where TModel : BaseModel
        {
            Type proxyType = Type.GetType(typeof(ProxyActivator).Namespace + "." + typeof(TModel).Name + "Proxy");
            return (TModel)Activator.CreateInstance(proxyType, dto);
        }

        public static void ReflectChangedProperty(object sender, PropertyChangedEventArgs args)
        {
            var proxy = sender as IDtoProxy;
            if (proxy != null)
            {
                var piSource = sender.GetType().GetProperty(args.PropertyName);
                var piTarget = proxy.Dto.GetType().GetProperty(args.PropertyName);
                if (piSource != null && piTarget != null && piSource.PropertyType == piTarget.PropertyType)
                {
                    piTarget.SetValue(proxy.Dto, piSource.GetValue(sender, null), null);
                }
            }
        }
    }
}
