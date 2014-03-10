using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace RF.WinApp
{
    public class RadioMenuHeader : MenuItem
    {
        public static readonly DependencyProperty DefaultKeyProperty = DependencyProperty.Register("DefaultKey", typeof(string), typeof(RadioMenuHeader), new UIPropertyMetadata("", null, OnCoerceDefaultKey));
        public static readonly DependencyProperty CurrentKeyProperty =
            DependencyProperty.Register("CurrentKey", typeof(string), typeof(RadioMenuHeader), new UIPropertyMetadata("", null, OnCoerceCurrentKey));

        private static object OnCoerceDefaultKey(DependencyObject target, object baseValue)
        {
            var mn = target as RadioMenuHeader;
            if (mn != null)
            {
                mn.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (string.IsNullOrEmpty(mn.CurrentKey))
                        mn.CurrentKeyChange((string)baseValue);
                }
                ), System.Windows.Threading.DispatcherPriority.Background);
            }

            return baseValue;
        }

        private static object OnCoerceCurrentKey(DependencyObject target, object baseValue)
        {
            var mn = target as RadioMenuHeader;
            if (mn != null)
            {
                if (mn.RadioChanged != null)
                {
                    mn.RadioChanged(mn, new RadioMenuEventArgs((string)baseValue));
                }
                mn.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (mn.CurrentKeyChange((string)baseValue) == null)
                        mn.CurrentKeyChange(mn.DefaultKey);
                }
                ), System.Windows.Threading.DispatcherPriority.Send);
            }
            return baseValue;
        }

        static RadioMenuHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioMenuHeader), new FrameworkPropertyMetadata(typeof(RadioMenuHeader)));
        }

        public string DefaultKey
        {
            get { return (string)GetValue(DefaultKeyProperty); }
            set { SetValue(DefaultKeyProperty, value); }
        }

        public string CurrentKey
        {
            get { return (string)GetValue(CurrentKeyProperty); }
            set { SetValue(CurrentKeyProperty, value); }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if (newValue != null)
                foreach (var radioItem in newValue.Cast<RadioMenuItem>().Where(i => i != null))
                {
                    radioItem.Selected -= this.ItemSelected;
                    radioItem.Selected += this.ItemSelected;
                }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
                foreach (var radioItem in e.NewItems.Cast<RadioMenuItem>().Where(i => i != null))
                {
                    radioItem.Selected -= this.ItemSelected;
                    radioItem.Selected += this.ItemSelected;
                }
        }

        static readonly RoutedEvent RadioChangedEvent = EventManager.RegisterRoutedEvent("RadioChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RadioMenuHeader));
        public event EventHandler<RadioMenuEventArgs> RadioChanged;
        protected void ItemSelected(object sender, EventArgs e)
        {
            var currentRadioItem = sender as RadioMenuItem;
            if (currentRadioItem != null)
                this.CurrentKey = currentRadioItem.Key;
        }

        internal RadioMenuItem CurrentKeyChange(string newKey)
        {
            RadioMenuItem ret = null;
            foreach (var item in this.Items)
            {
                var mnItem = item as RadioMenuItem;
                if (mnItem != null)
                {
                    if (mnItem.Key == newKey)
                    {
                        ret = mnItem;
                        mnItem.IsChecked = true;
                        if (RadioChanged != null && this.CurrentKey != newKey)
                        {
                            RadioChanged(this, new RadioMenuEventArgs(mnItem.Key));
                        }
                    }
                    else
                    {
                        mnItem.IsChecked = false;
                    }
                }
            }
            return ret;
        }
    }

    public class RadioMenuEventArgs : EventArgs
    {
        public string CurrentKey { get; private set; }
        public RadioMenuEventArgs(string key)
        {
            this.CurrentKey = key;
        }
    }
}
