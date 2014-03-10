using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RF.WinApp.UC
{
    /// <summary>
    /// Interaction logic for DateUC.xaml
    /// </summary>
    public partial class DateUC : UserControl
    {
        public static readonly DependencyProperty DateValueProperty;

        static DateUC()
        {
            DateValueProperty = DependencyProperty.Register("DateValue", typeof(DateTime?), typeof(DateUC), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
            ToggleCalendarAction = new RoutedCommand("ToggleCalendar", typeof(DateUC));
        }

        private CommandBinding toggleCalendarAction;

        public DateUC()
        {
            InitializeComponent();
            toggleCalendarAction = new CommandBinding(ToggleCalendarAction, ToggleCalendar);
            CommandBindings.Add(toggleCalendarAction);
        }

        public DateTime? DateValue
        {
            get { return (DateTime?)GetValue(DateValueProperty); }
            set { SetValue(DateValueProperty, value); }
        }

        public static RoutedCommand ToggleCalendarAction { get; private set; }
        protected virtual void ToggleCalendar(object sender, ExecutedRoutedEventArgs e)
        {
            calDatePopup.IsOpen = true;
            //Mouse.Capture(mdpDate, CaptureMode.None);
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(calDatePopup, new MouseButtonEventHandler(mdpDate_PreviewMouseDownOutsideCapturedElement));
            System.Diagnostics.Debug.WriteLine("ToggleCalendarAction");
        }

        private void mdpDate_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            calDatePopup.StaysOpen = false;
            System.Diagnostics.Debug.WriteLine("mdpDate_MouseLeftButtonDown");
        }

        private void mdpDate_PreviewMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
        {
            calDatePopup.StaysOpen = IsMouseClickWithin(this.mdpDate, e.MouseDevice.GetPosition(this.mdpDate));
            System.Diagnostics.Debug.WriteLine("mdpDate_PreviewMouseDownOutsideCapturedElement");
        }

        private bool IsMouseClickWithin(FrameworkElement element, Point point)
        {
            return element.ActualWidth >= point.X && point.X >= 0 && element.ActualHeight >= point.Y && point.Y >=0;
        }
    }

    internal class Nullabledate2DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? DateTime.Today;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
