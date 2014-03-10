using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;

using RF.WinApp.Infrastructure.CC;

namespace RF.WinApp.Infrastructure.Behaviour
{
    internal static class XamlHelper
    {
        public static T FindAncestor<T>(Visual visual) where T : Visual
        {
            DependencyObject p = visual;
            while (p != null && p != Application.Current.MainWindow)
            {
                p = VisualTreeHelper.GetParent(p);
                if (p is T)
                    return p as T;
            }

            return null;
        }
    }
}
