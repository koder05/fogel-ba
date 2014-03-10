using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using System.Reflection;
using System.Linq;

using RF.WinApp.Infrastructure.Behaviour;
using RF.WinApp.JIT;
using RF.BL.Model;

namespace RF.WinApp
{
    public class ActionTextBox : TextBox
    {
        public readonly static DependencyProperty ActionProperty = DependencyProperty.RegisterAttached("Action", typeof(ICommand), typeof(ActionTextBox), new UIPropertyMetadata(null));

        static ActionTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActionTextBox), new FrameworkPropertyMetadata(typeof(ActionTextBox)));
        }

        public ICommand Action
        {
            get { return (ICommand)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }
    }
}
