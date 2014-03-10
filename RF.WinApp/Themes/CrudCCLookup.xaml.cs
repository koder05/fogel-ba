using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;

using RF.WinApp.Infrastructure.Behaviour;

namespace RF.WinApp
{
    public partial class CrudCCLookup : ResourceDictionary
    {
        public CrudCCLookup()
        {
            InitializeComponent();
        }

        private void PART_Button_Click(object sender, RoutedEventArgs e)
        {
            var window = ((FrameworkElement)sender).TemplatedParent;
            var aPopup = DispatcherHelper.FindVisualChildren<AdornedPopup>(window).FirstOrDefault(fe => fe.Name == "aPopup");
            var contentHost = DispatcherHelper.FindVisualChildren<TextBox>(window).FirstOrDefault(fe => fe.Name == "ContentHost");

            ContentHost_LostFocus(contentHost, null);

            aPopup.IsShow = !aPopup.IsShow;
        }

        private void ContentHost_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.DataContext = null;
            }
        }

        private void ContentHost_Initialized(object sender, EventArgs e)
        {
            var window = ((FrameworkElement)sender).TemplatedParent;
            var crud = window as RF.WinApp.CrudCC;
            var binding = new Binding();
            binding.Path = new PropertyPath(crud.GetValue(CrudLookup.LookupFieldProperty));
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(sender as TextBox, TextBox.TextProperty, binding);

            crud.PropertyChanged += Crud_PropertyChangedEventHandler;
        }

        private void Crud_PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedModel")
            {
                var window = (FrameworkElement)sender;
                var aPopup = DispatcherHelper.FindVisualChildren<AdornedPopup>(window).FirstOrDefault(fe => fe.Name == "aPopup");
                aPopup.IsShow = false;
            }
        }
    }

    internal class Object2ModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dobj = value as RF.WinApp.JIT.DataObj;
            if (dobj != null)
                value = dobj.Model;
            return value;
        }
    }
}
