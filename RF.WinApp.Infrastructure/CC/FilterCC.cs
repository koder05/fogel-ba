using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel; 

using RF.LinqExt;

namespace RF.WinApp
{
    public class FilterCC : Control
    {
        public static readonly DependencyProperty OperatorTypeProperty;
        public static readonly DependencyProperty FieldNameProperty;
        public static readonly DependencyProperty ValueProperty;

        static FilterCC()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterCC), new FrameworkPropertyMetadata(typeof(FilterCC)));

            OperatorTypeProperty = DependencyProperty.Register("OperatorType", typeof(OperatorType), typeof(FilterCC), new UIPropertyMetadata(null));
            FieldNameProperty = DependencyProperty.Register("FieldName", typeof(string), typeof(FilterCC), new UIPropertyMetadata(null));
            ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(FilterCC), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        }

        [Description("The image displayed by the button"), Category("Common Properties")]
        public OperatorType OperatorType
        {
            get { return (OperatorType)GetValue(OperatorTypeProperty); }
            set { SetValue(OperatorTypeProperty, value); }
        }

        public string FieldName
        {
            get { return (string)GetValue(FieldNameProperty); }
            set { SetValue(FieldNameProperty, value); }
        }

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}
