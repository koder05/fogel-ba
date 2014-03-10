using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Windows.Input;

namespace RF.WinApp
{
    [TemplatePart(Name = TabsTemplatePartName, Type = typeof(TabControl))]
    public class TabsPlatesCC : ItemsControl
    {
        private static readonly object sync = new object();
        private static bool OnAddCreateTab = true;

        private const string TabsTemplatePartName = "PART_Tabs";

        public static RoutedCommand ItemCloseAction { get; private set; }

        public static void SetAddingOrder(bool isCreateTab)
        {
            lock (sync)
                OnAddCreateTab = isCreateTab;
        }

        private Dictionary<PlatesCC, ObservableCollection<object>> platesItems = new Dictionary<PlatesCC, ObservableCollection<object>>();

        static TabsPlatesCC()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabsPlatesCC), new FrameworkPropertyMetadata(typeof(TabsPlatesCC)));
            ItemCloseAction = new RoutedCommand("ItemCloseAction", typeof(TabsPlatesCC));
        }

        public TabsPlatesCC()
        {
            this.Items.CurrentChanged += Items_CurrentChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CommandBindings.Add(new CommandBinding(ItemCloseAction, ItemClose));

            var tabs = Template.FindName(TabsTemplatePartName, this) as TabControl;
            foreach (var plates in this.platesItems.Keys)
            {
                var viewParent = ((FrameworkElement)plates).Parent as TabControl;
                if (viewParent != null)
                    viewParent.Items.Clear();
                tabs.Items.Add(plates);

                if (platesItems[plates].Any(i => i == this.Items.CurrentItem))
                    tabs.SelectedItem = plates;
            }
        }

        private void Items_CurrentChanged(object sender, EventArgs e)
        {
            var tabs = Template.FindName(TabsTemplatePartName, this) as TabControl;

            //if (tabs != null)                 tabs.Items.Refresh();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            var tabs = Template.FindName(TabsTemplatePartName, this) as TabControl;
            if (e.NewItems != null && tabs != null)
            {
                foreach (var view in e.NewItems)
                {
                    if (tabs.Items.Count == 0 || OnAddCreateTab)
                    {
                        CreateNewTab();
                    }

                    var currentPlates = tabs.SelectedItem as PlatesCC;
                    var currentPlatesItems = this.platesItems[currentPlates];
                    currentPlatesItems.Add(view);
                }
            }
        }

        private void CreateNewTab()
        {
            var tabs = Template.FindName(TabsTemplatePartName, this) as TabControl;
            if (tabs != null)
            {
                PlatesCC plates = new PlatesCC();
                plates.RemovePlate += PlatesCC_RemovePlate;
                var list = new List<object>();
                var items = new ObservableCollection<object>(list);
                plates.ItemsSource = items;
                tabs.Items.Add(plates);
                tabs.SelectedItem = plates;
                this.platesItems.Add(plates, items);
            }
        }

        private void PlatesCC_RemovePlate(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null && e.OldItems.Count > 0)
            {
                var plates = sender as PlatesCC;
                if (plates != null)
                    foreach (var view in e.OldItems)
                        this.platesItems[plates].Remove(view);
                OnRemoveItems(e);
            }
        }

        public event EventHandler<NotifyCollectionChangedEventArgs> RemoveItems;
        public void OnRemoveItems(NotifyCollectionChangedEventArgs e)
        {
            if (RemoveItems != null)
            {
                RemoveItems(this, e);
            }
        }

        public virtual void ItemClose(object sender, ExecutedRoutedEventArgs e)
        {
            var tabs = Template.FindName(TabsTemplatePartName, this) as TabControl;
            PlatesCC platesToRemove = e.Parameter as PlatesCC;
            NotifyCollectionChangedEventArgs removeArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, this.platesItems[platesToRemove]);

            platesToRemove.RemovePlate -= PlatesCC_RemovePlate;

            tabs.Items.Remove(platesToRemove);
            this.platesItems.Remove(platesToRemove);

            OnRemoveItems(removeArgs);
            platesToRemove.Free();
        }

        public void SetWaitOverdoor(bool on)
        {
            var tabs = Template.FindName(TabsTemplatePartName, this) as TabControl;
            if (tabs != null)
            {
                PlatesCC activePlates = tabs.SelectedItem as PlatesCC;
                if (activePlates != null)
                    activePlates.SetWaitOverdoor(on);
                //else                    SetSelfWaitOverdoor(on);
            }
        }

        private const string WaitOverdoorTemplatePartName = "PART_WaitOverdoor";
        private const string WaitImgTemplatePartName = "PART_GifWait";
        private void SetSelfWaitOverdoor(bool on)
        {
            var overdoor = Template.FindName(WaitOverdoorTemplatePartName, this) as UIElement;
            if (overdoor != null)
            {
                overdoor.Visibility = on ? Visibility.Visible : Visibility.Collapsed;
            }

            var gif = Template.FindName(WaitImgTemplatePartName, this) as UIElement;
            if (gif != null)
            {
                gif.Visibility = on ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}
