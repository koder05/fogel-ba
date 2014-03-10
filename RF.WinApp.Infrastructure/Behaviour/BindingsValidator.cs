using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;

namespace RF.WinApp.Infrastructure.Behaviour
{
    public static class BindingsValidator
    {
        public static bool ValidateAll(this DependencyObject o)
        {
            bool validateFail = false;

            List<FieldInfo> propertiesAll = new List<FieldInfo>();
            Type currentLevel = o.GetType();
            while (currentLevel != typeof(object))
            {
                propertiesAll.AddRange(currentLevel.GetFields());
                currentLevel = currentLevel.BaseType;
            }
            var propertiesDp = propertiesAll.Where(x => x.FieldType == typeof(DependencyProperty));
            foreach (var property in propertiesDp)
            {
                BindingExpression ex = BindingOperations.GetBindingExpression(o, property.GetValue(o) as DependencyProperty);
                if (ex != null)
                {
                    ex.UpdateSource();
                    validateFail |= ex.HasError;
                }
            }

            //Children
            int childrenCount = VisualTreeHelper.GetChildrenCount(o);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);
                validateFail |= !child.ValidateAll();
            }

            return !validateFail;
        }

    }
}
