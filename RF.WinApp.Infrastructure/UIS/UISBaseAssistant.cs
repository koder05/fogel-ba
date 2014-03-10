using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RF.WinApp.Infrastructure.UIS
{
    public abstract class UISBaseAssistant : IUISettingsTypeAssistant
    {
        protected UISettingsStoreProvider StoreProvider;
        
        public UISBaseAssistant(UISettingsStoreProvider _storeProvider)
        {
            StoreProvider = _storeProvider;
        }
        
        public abstract Type AttendedType { get; }

        public abstract void EventsSubscribe(object target, string controlUID);

        public virtual void AttendInstance(object target, string controlUID)
        {
            var dobj = target as DependencyObject;
            if (dobj != null && StoreProvider != null)
            {
                var settings = StoreProvider.GetSettings(controlUID);
                if (settings != null)
                    foreach (var kvp in settings)
                    {
                        var descriptor = System.ComponentModel.DependencyPropertyDescriptor.FromName(kvp.Key, AttendedType, AttendedType);
                        if (descriptor != null)
                            dobj.SetValue(descriptor.DependencyProperty, kvp.Value);
                    }
            }
        }
    }
}
