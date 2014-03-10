using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using RF.WinApp.Infrastructure.Behaviour;

namespace RF.WinApp.UC
{
    /// <summary>
    /// Interaction logic for DateRangeUC.xaml
    /// </summary>
    public partial class DateRangeUC : UserControl
    {
        public static readonly DependencyProperty TillDateProperty;
        public static readonly DependencyProperty FromDateProperty;

        static DateRangeUC()
        {
            TillDateProperty = DependencyProperty.Register("TillDate", typeof(DateTime?), typeof(DateRangeUC), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
            FromDateProperty = DependencyProperty.Register("FromDate", typeof(DateTime?), typeof(DateRangeUC), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
        }

        public DateRangeUC()
        {
            InitializeComponent();
            //EventManager.RegisterClassHandler(typeof(DatePicker), DatePicker.LoadedEvent, new RoutedEventHandler(DatePicker_Loaded));
            this.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(ValidationErrorEventHandler));
            this.SetValue(Validation.ErrorTemplateProperty, null);
        }

        public DateTime? TillDate
        {
            get { return (DateTime?)GetValue(TillDateProperty); }
            set { SetValue(TillDateProperty, value); }
        }

        public DateTime? FromDate
        {
            get { return (DateTime?)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }

        private void ValidationErrorEventHandler(object sender, ValidationErrorEventArgs e)
        {
            if (e.Error.BindingInError is BindingExpression)
            {
                if (e.Error.BindingInError == BindingOperations.GetBindingExpression(this, DateRangeUC.FromDateProperty))
                {
                    this.mdpDateFrom.BindingGroup.ValidationRules.Clear();
                    this.mdpDateFrom.BindingGroup.ValidationRules.Add(new BubbleErrorValidationRule(e.Error));
                    this.mdpDateFrom.BindingGroup.ValidateWithoutUpdate();
                }
                else if (e.Error.BindingInError == BindingOperations.GetBindingExpression(this, DateRangeUC.TillDateProperty))
                {
                    this.mdpDateTo.BindingGroup.ValidationRules.Clear();
                    this.mdpDateTo.BindingGroup.ValidationRules.Add(new BubbleErrorValidationRule(e.Error));
                    this.mdpDateTo.BindingGroup.ValidateWithoutUpdate();
                }
            }
        }
    }
}
