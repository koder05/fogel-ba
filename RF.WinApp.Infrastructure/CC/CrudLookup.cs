using System;
using System.Reflection;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Markup;

using RF.WinApp.Infrastructure;
using RF.WinApp.Infrastructure.Behaviour;

namespace RF.WinApp
{
    [TemplatePart(Name = "PART_LookupBox", Type = typeof(TextBox))]
    public class CrudLookup
    {
        public readonly static DependencyProperty IsLookupProperty = DependencyProperty.RegisterAttached("IsLookup", typeof(bool), typeof(CrudLookup), new UIPropertyMetadata(false, null, OnCoerceIsLookup));
        public readonly static DependencyProperty LookupWidthProperty = DependencyProperty.RegisterAttached("LookupWidth", typeof(double), typeof(CrudLookup), new UIPropertyMetadata(50.0, null, OnCoerceLookupWidth));
        public static readonly DependencyProperty LookupValueProperty = DependencyProperty.RegisterAttached("LookupValue", typeof(object), typeof(CrudLookup), new UIPropertyMetadata(null, null, OnCoerceLookupValue));
        public static readonly DependencyProperty LookupFieldProperty = DependencyProperty.RegisterAttached("LookupField", typeof(string), typeof(CrudLookup), new UIPropertyMetadata(null));
        public static readonly DependencyProperty LookupStyleProperty = DependencyProperty.RegisterAttached("LookupStyle", typeof(Style), typeof(CrudLookup), new UIPropertyMetadata(null, null, OnCoerceLookupStyle));

        public static bool GetIsLookup(DependencyObject target)
        {
            return (bool)target.GetValue(IsLookupProperty);
        }
        public static void SetIsLookup(DependencyObject target, bool value)
        {
            target.SetValue(IsLookupProperty, value);
        }
        private static object OnCoerceIsLookup(DependencyObject target, object baseValue)
        {
            var lookup = target as ICrudLookupMode;
            if (lookup != null && (bool)baseValue)
            {
                var uc = target as UIElement;
                if (uc != null)
                {
                    uc.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(ValidationErrorEventHandler));
                    BindingGroup bg = new BindingGroup();
                    lookup.LookupControl.BindingGroup = bg;
                    uc.SetValue(Validation.ErrorTemplateProperty, null);
                }

                lookup.LookupControl.TemplateApplied -= LookupControl_TemplateApplied;
                lookup.LookupControl.TemplateApplied += LookupControl_TemplateApplied;

                lookup.LookupControl.Dispatcher.BeginInvoke(new Action(() => {
                    var style = GetLookupStyle(target) ?? lookup.LookupControl.TryFindResource("Lookup") as Style;
                    if (style != null && style != lookup.LookupControl.Style)
                        lookup.LookupControl.SetValue(FrameworkElement.StyleProperty, style);
                }),  System.Windows.Threading.DispatcherPriority.Background);
            }

            return baseValue;
        }

        public static Style GetLookupStyle(DependencyObject target)
        {
            return (Style)target.GetValue(LookupStyleProperty);
        }
        public static void SetLookupStyle(DependencyObject target, Style value)
        {
            target.SetValue(LookupStyleProperty, value);
        }
        private static object OnCoerceLookupStyle(DependencyObject target, object baseValue)
        {
            return baseValue;
        }

        private static void LookupControl_TemplateApplied(object sender, EventArgs e)
        {
            var crud = sender as CrudCC;
            var tb = crud.Template.FindName("PART_LookupBox", crud) as TextBox;
            if (tb != null)
            {
                tb.PreviewKeyUp += PART_LookupBox_KeyUp;
                var binding = new Binding(crud.GetValue(CrudLookup.LookupFieldProperty) as string);
                binding.Mode = BindingMode.OneWay;
                BindingOperations.SetBinding(tb, TextBox.TextProperty, binding);
            }
        }

        private static void PART_LookupBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.DataContext = null;
            }
        }

        public static double GetLookupWidth(DependencyObject target)
        {
            return (double)target.GetValue(LookupWidthProperty);
        }
        public static void SetLookupWidth(DependencyObject target, double value)
        {
            target.SetValue(LookupWidthProperty, value);
        }

        private static object OnCoerceLookupWidth(DependencyObject target, object baseValue)
        {
            var lookup = target as ICrudLookupMode;
            if (lookup != null)
            {
                lookup.LookupControl.SetValue(CrudLookup.LookupWidthProperty, (double)baseValue);
            }

            return baseValue;
        }

        public static object GetLookupField(DependencyObject target)
        {
            return target.GetValue(LookupFieldProperty);
        }
        public static void SetLookupField(DependencyObject target, object value)
        {
            target.SetValue(LookupFieldProperty, value);
        }

        private static object OnCoerceLookupValue(DependencyObject target, object baseValue)
        {
            ICrudLookupMode lookup = target as ICrudLookupMode;
            if (lookup != null)
            {
                lookup.LookupControl.SelectedModel = baseValue;
                lookup.LookupControl.PropertyChanged -= SelectedModelChangedHandler;
                lookup.LookupControl.PropertyChanged += SelectedModelChangedHandler;
            }

            return baseValue;
        }

        public static object GetLookupValue(DependencyObject target)
        {
            return target.GetValue(LookupValueProperty);
        }
        public static void SetLookupValue(DependencyObject target, object value)
        {
            target.SetValue(LookupValueProperty, value);
        }

        private static void SelectedModelChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var crud = sender as CrudCC;
            if (e.PropertyName == "SelectedModel" && crud != null)
            {
                if (crud.SelectedModel != null)
                {
                    var isshow = crud.GetValue(AdornedPopup.IsShowProperty) as bool?;
                    if (isshow != null && isshow.Value)
                        crud.SetValue(AdornedPopup.IsShowProperty, false);
                }

                if (crud.Parent is ICrudLookupMode)
                    crud.Parent.SetCurrentValue(CrudLookup.LookupValueProperty, crud.SelectedModel);
            }
        }

        private static void ValidationErrorEventHandler(object sender, ValidationErrorEventArgs e)
        {
            var lookup = sender as ICrudLookupMode;
            if (lookup != null)
            {
                lookup.LookupControl.BindingGroup.ValidationRules.Clear();
                lookup.LookupControl.BindingGroup.ValidationRules.Add(new BubbleErrorValidationRule(e.Error));
                lookup.LookupControl.BindingGroup.ValidateWithoutUpdate();
            }
        }
    }

    public class LookupFieldValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var parameters = parameter as object[];
            if (value != null && parameters != null && parameters.Length == 2)
            {
                var crud = parameters[0] as CrudCC;
                if (crud != null)
                {
                    string lookupFieldName = crud.GetValue(CrudLookup.LookupFieldProperty) as string;
                    if (!string.IsNullOrEmpty(lookupFieldName))
                    {
                        var type = value.GetType();
                        var propInfo = type.GetProperty(lookupFieldName);
                        if (propInfo != null)
                            return propInfo.GetValue(value, null);
                    }
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            var parameters = parameter as object[];
            if (parameters != null && parameters.Length == 2)
            {
                var crud = parameters[0] as CrudCC;
                if (crud != null)
                {
                    string lookupFieldValue = value as string;
                    if (string.IsNullOrWhiteSpace(lookupFieldValue))
                    {
                        return null;
                    }
                    else
                    {
                        return crud.SelectedModel;
                    }
                }
            }

            return null;
        }
    }
}
