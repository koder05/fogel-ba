using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;

namespace RF.WinApp.UC
{
    public partial class DateInputUC : UserControl
    {
        public static readonly DependencyProperty DateValueProperty;

        static DateInputUC()
        {
            DateValueProperty = DependencyProperty.Register("DateValue", typeof(DateTime?), typeof(DateInputUC), new UIPropertyMetadata(null, null, OnCoerceDateValue));
            ToggleCalendarAction = new RoutedCommand("ToggleCalendar", typeof(DateInputUC));
        }

        private static object OnCoerceDateValue(DependencyObject target, object baseValue)
        {
            return baseValue;
        }

        private CommandBinding toggleCalendarAction;

        public DateInputUC()
        {
            InitializeComponent();
            toggleCalendarAction = new CommandBinding(ToggleCalendarAction, ToggleCalendar);
            CommandBindings.Add(toggleCalendarAction);
            this.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(ValidationErrorEventHandler));
        }

        public DateTime? DateValue
        {
            get { return (DateTime?)GetValue(DateValueProperty); }
            set { SetValue(DateValueProperty, value); }
        }

        public static RoutedCommand ToggleCalendarAction { get; private set; }
        protected virtual void ToggleCalendar(object sender, ExecutedRoutedEventArgs e)
        {
            aPopup.IsShow = !aPopup.IsShow;
        }

        private void ValidationErrorEventHandler(object sender, ValidationErrorEventArgs e)
        {
            //mdpDate.BindingGroup.ValidationRules.Clear();
            //mdpDate.BindingGroup.ValidationRules.Add(new BubbleErrorValidationRule(e.Error.ErrorContent));
            //mdpDate.BindingGroup.ValidateWithoutUpdate();
        }

        private class BubbleErrorValidationRule : ValidationRule
        {
            private object _error;
            public BubbleErrorValidationRule(object error)
            {
                _error = error;
            }

            public override ValidationResult Validate(object value, CultureInfo cultureInfo)
            {
                return new ValidationResult(false, _error);
            }
        }

        private void calDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var cal = sender as System.Windows.Controls.Calendar;

            if (cal != null && cal.SelectedDate != null && (cal.DisplayDate.Month != cal.SelectedDate.Value.Month || cal.DisplayDate.Year != cal.SelectedDate.Value.Year))
            {
                var db = new DateTime(cal.DisplayDate.Year, cal.DisplayDate.Month, 1).AddDays(-1);
                while (db.DayOfWeek != cal.FirstDayOfWeek)
                {
                    db = db.AddDays(-1);
                }

                var de = db.AddDays(6 * 7 - 1);

                //System.Diagnostics.Debug.WriteLine(string.Format("BeginDate={0}, EndDate={1}", db, de));

                if (!(cal.SelectedDate.Value >= db && cal.SelectedDate.Value <= de))
                    cal.DisplayDate = cal.SelectedDate.Value;

            }
        }
    }

    internal class Date2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return ((DateTime)value).ToShortDateString();
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (string.IsNullOrEmpty(strValue) == false)
            {
                DateTime resultDateTime;
                if (DateTime.TryParse(strValue, out resultDateTime))
                    return resultDateTime;
            }
            return (DateTime?)null;
        }
    }
}
