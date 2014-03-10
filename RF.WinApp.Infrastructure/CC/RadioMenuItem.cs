using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;

namespace RF.WinApp
{
    public class RadioMenuItem : MenuItem
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(string), typeof(RadioMenuItem), new UIPropertyMetadata(""));

        static RadioMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioMenuItem), new FrameworkPropertyMetadata(typeof(RadioMenuItem)));
        }

        public RadioMenuItem(string header, string key)
            : this()
        {
            this.Header = header;
            this.Key = key;
            var style = Application.Current.TryFindResource(typeof(RadioMenuItem)) as Style;
            this.SetValue(FrameworkElement.StyleProperty, style);
        }

        public RadioMenuItem()
        {
            this.Command = new DelegateCommand(OnSelected);
        }

        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public event EventHandler Selected;
        private void OnSelected()
        {
            if (Selected != null)
            {
                Selected(this, EventArgs.Empty);
            }
        }
    }
}
