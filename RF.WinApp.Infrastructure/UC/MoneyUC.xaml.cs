using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RF.WinApp.UC
{
    public partial class MoneyUC : UserControl
    {
        public static readonly DependencyProperty MoneyValueProperty;

        static MoneyUC()
        {
            MoneyValueProperty = DependencyProperty.Register("MoneyValue", typeof(decimal), typeof(MoneyUC), new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.Inherits));
        }

        public MoneyUC()
        {
            InitializeComponent();
        }

        public decimal MoneyValue
        {
            get { return (decimal)GetValue(MoneyValueProperty); }
            set { SetValue(MoneyValueProperty, value); }
        }

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                var be = BindingOperations.GetBindingExpression(tb, TextBox.TextProperty);
                (be.ParentBinding.Converter as Money2StrConverter).Format = "{0:#0.00}";
                be.UpdateTarget();
            }
        }

        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                var be = BindingOperations.GetBindingExpression(tb, TextBox.TextProperty);
                (be.ParentBinding.Converter as Money2StrConverter).Format = string.Empty;
                be.UpdateTarget();
            }
        }
    }

    internal class Money2StrConverter : IValueConverter
    {
        public string Format { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format((string.IsNullOrEmpty(Format) ? "{0:n2}" : Format), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal res = 0;
            //if (decimal.TryParse((string)value, out res) == false) return new InvalidCastException("Требуется числовой формат");
            decimal.TryParse((string)value, out res);
            return res;
        }
    }

    internal class MoneyUCValidationRule : ValidationRule 
    {  
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            decimal res = 0;
            if (decimal.TryParse((string)value, out res) == false) 
            {
                return new ValidationResult(false, "Требуется числовой формат");
            }

            return ValidationResult.ValidResult;
        }
    }
}
