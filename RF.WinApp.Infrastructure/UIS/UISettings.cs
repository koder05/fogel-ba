using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace RF.WinApp.Infrastructure.UIS
{
    public static class UISettings
    {
        private static Dictionary<Type, IUISettingsTypeAssistant> _typeAssistants = new Dictionary<Type, IUISettingsTypeAssistant>();
        public readonly static DependencyProperty ControlUIDProperty = DependencyProperty.RegisterAttached("ControlUID", typeof(string), typeof(UISettings), new UIPropertyMetadata(null, null, OnCoerceControlUID));

        public static void RegisterTypeAssistant(IUISettingsTypeAssistant assistant)
        {
            if (assistant != null && !_typeAssistants.ContainsKey(assistant.AttendedType))
                _typeAssistants.Add(assistant.AttendedType, assistant);
        }

        public static string GetControlUID(DependencyObject target)
        {
            return (string)target.GetValue(ControlUIDProperty);
        }

        public static void SetControlUID(DependencyObject target, string value)
        {
            target.SetValue(ControlUIDProperty, value);
        }

        private static object OnCoerceControlUID(DependencyObject target, object baseValue)
        {
            var uid = baseValue as string;

            if (string.IsNullOrWhiteSpace(uid))
                throw new InvalidOperationException("UID клиентских настроек элемента интерфейса не может быть пустым.");
            
            Type targetType = target.GetType();
            if (_typeAssistants.ContainsKey(targetType))
            {
                var assistant = _typeAssistants[targetType];
                assistant.AttendInstance(target, uid);
                assistant.EventsSubscribe(target, uid);
            }

            return baseValue;
        }
    }
}
