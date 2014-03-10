using System;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

using RF.WinApp.Infrastructure;
using RF.WinApp.Infrastructure.Behaviour;

namespace RF.WinApp
{
    public class ViewName
    {
        public readonly static DependencyProperty TitleProperty = DependencyProperty.RegisterAttached("Title", typeof(string), typeof(ViewName), new UIPropertyMetadata("Без имени"));

        public static object GetTitle(DependencyObject target)
        {
            return target.GetValue(TitleProperty);
        }

        public static void SetTitle(DependencyObject target, string value)
        {
            target.SetValue(TitleProperty, value);
        }
    }
}
