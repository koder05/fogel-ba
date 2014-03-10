using System;
using System.Collections.Generic;
using System.Windows;

namespace RF.WinApp.Infrastructure.UIS
{
    public class UISettingsAssistantWindow : IUISettingsTypeAssistant
    {
        private UISettingsStoreProvider _storeProvider;
        public virtual Type AttendedType
        {
            get { return typeof(Window); }
        }

        public void EventsSubscribe(object target, string controlUID)
        {
            var window = target as Window;
            window.Closed += WindowClosed;
        }

        public void AttendInstance(object target, string controlUID)
        {
            var window = target as Window;
            if (window != null && _storeProvider != null)
            {
                var settings = _storeProvider.GetSettings(controlUID);
                if (settings != null)
                    foreach (var kvp in settings)
                    {
                        var descriptor = System.ComponentModel.DependencyPropertyDescriptor.FromName(kvp.Key, AttendedType, AttendedType);
                        if (descriptor != null)
                            window.SetValue(descriptor.DependencyProperty, kvp.Value);
                    }
            }
        }

        public UISettingsAssistantWindow(UISettingsStoreProvider storeProvider)
        {
            _storeProvider = storeProvider;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (window != null && _storeProvider != null)
            {
                var uid = window.GetValue(UISettings.ControlUIDProperty) as string;
                var settings = new Dictionary<string, object>();
                settings.Add(Window.WidthProperty.Name, window.GetValue(Window.WidthProperty));
                settings.Add(Window.HeightProperty.Name, window.GetValue(Window.HeightProperty));
                settings.Add(Window.LeftProperty.Name, window.GetValue(Window.LeftProperty));
                settings.Add(Window.TopProperty.Name, window.GetValue(Window.TopProperty));
                settings.Add(Window.WindowStateProperty.Name, window.GetValue(Window.WindowStateProperty));
                _storeProvider.PutSettings(uid, settings);
            }
        }
    }
}
