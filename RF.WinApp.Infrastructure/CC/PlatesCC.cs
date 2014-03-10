using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RF.WinApp
{
    [TemplatePart(Name = ItemsPanelTemplatePartName, Type = typeof(Grid))]
    [TemplatePart(Name = WidgetsPanelTemplatePartName, Type = typeof(UniformGrid))]
    [TemplatePart(Name = MainPanelTemplatePartName, Type = typeof(Border))]
    public class PlatesCC : ItemsControl
    {
        private const string ItemsPanelTemplatePartName = "PART_ItemsPanel";
        private const string WidgetsPanelTemplatePartName = "PART_WidgetsPanel";
        public const string MainPanelTemplatePartName = "PART_MainPlaceHolder";
        private const string WaitOverdoorTemplatePartName = "PART_WaitOverdoor";
        private const string WaitImgTemplatePartName = "PART_GifWait";

        static PlatesCC()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlatesCC), new FrameworkPropertyMetadata(typeof(PlatesCC)));
        }

        public PlatesCC()
        {
            this.Items.CurrentChanged += Items_CurrentChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            foreach (var view in this.Items)
            {
                var viewParent = ((FrameworkElement)view).Parent as Decorator;
                if (viewParent != null)
                    viewParent.Child = null;

                if (view != this.Items.CurrentItem)
                    AddItem(view as UIElement);
            }

            Items_CurrentChanged(this, EventArgs.Empty);
        }

        public void Free()
        {
            this.Items.CurrentChanged -= Items_CurrentChanged;
            this.ItemsSource = null;
            this.CommandBindings.Clear();

            var main = Template.FindName(MainPanelTemplatePartName, this) as Border;
            main.Child = null;

            var widgets = Template.FindName(WidgetsPanelTemplatePartName, this) as UniformGrid;
            foreach (var widget in widgets.Children)
            {
                var scb = widget as ScaleBoxCC;
                scb.Activate -= Widget_Activate;
                scb.Closed -= Widget_Close;
                scb.CommandBindings.Clear();
                scb.Child = null;
            }
            widgets.Children.Clear();
        }

        public void SetWaitOverdoor(bool on)
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

        private void Items_CurrentChanged(object sender, EventArgs e)
        {
            AddItem(this.Items.CurrentItem as UIElement);
        }

        private void AddItem(UIElement view)
        {
            if (Template == null)
                return;

            var container = Template.FindName(ItemsPanelTemplatePartName, this) as Grid;
            var main = Template.FindName(MainPanelTemplatePartName, this) as Border;
            var widgets = Template.FindName(WidgetsPanelTemplatePartName, this) as UniformGrid;

            if (main != null && view != null)
            {
                this.SetValue(ViewName.TitleProperty, view.GetValue(ViewName.TitleProperty));
                
                ScaleBoxCC widgetToRemove = widgets.Children.Cast<ScaleBoxCC>().FirstOrDefault(widget => widget.Child == view);
                if (widgetToRemove != null)
                {
                    widgetToRemove.Child = null;
                    widgets.Children.Remove(widgetToRemove);
                }

                var oldview = main.Child as FrameworkElement;
                main.Child = view;

                (view as FrameworkElement).Height = Double.NaN;
                (view as FrameworkElement).Width = Double.NaN;

                if (oldview != null)
                {
                    var newWidget = new ScaleBoxCC(oldview);
                    widgets.Children.Add(newWidget);
                    newWidget.Activate += Widget_Activate;
                    newWidget.Closed += Widget_Close;
                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() => this.SetWidgetsArea()), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

                //if (widgets.Children.Count > 0 && container.ColumnDefinitions[0].Width.Value < 2)
                //{
                //    container.ColumnDefinitions[0].Width = new GridLength(container.ActualWidth / 4);
                //}

                //if (widgets.Children.Count > widgets.Columns * widgets.Rows)
                //{
                //    var actualCellHeight = widgets.ActualHeight / widgets.Columns;
                //    var actualCellWidth = widgets.ActualWidth / widgets.Rows;
                //    var previewActualCellHeight = widgets.ActualHeight / (widgets.Rows + 1);
                //    var previewActualCellWidth = widgets.ActualWidth / (widgets.Columns + 1);

                //    if (Math.Abs(previewActualCellHeight - actualCellWidth) < Math.Abs(previewActualCellWidth - actualCellHeight))
                //    {
                //        widgets.Rows++;
                //    }
                //    else
                //    {
                //        widgets.Columns++;
                //    }
                //}
            }
        }

        protected void SetWidgetsArea()
        {
            if (Template == null)
                return;

            var container = Template.FindName(ItemsPanelTemplatePartName, this) as Grid;
            var widgets = Template.FindName(WidgetsPanelTemplatePartName, this) as UniformGrid;

            if (widgets.Children.Count == 0)
            {
                container.ColumnDefinitions[0].Width = new GridLength(1);
            }
            else if (container.ColumnDefinitions[0].Width.Value < 2)
            {
                container.ColumnDefinitions[0].Width = new GridLength(container.ActualWidth / 4);
            }

            if (widgets.Children.Count > 0 && widgets.Children.Count != widgets.Columns * widgets.Rows)
            {
                int widgetsRows = 0;
                int widgetsCols = 0;
                var widgetsRowsNext = widgets.Children.Count;
                var widgetsColsNext = 1;
                var delta = double.MaxValue;
                while (widgetsRowsNext <= widgets.Children.Count && widgetsColsNext <= widgets.Children.Count 
                            && Math.Abs(widgets.ActualHeight / widgetsRowsNext - widgets.ActualWidth / widgetsColsNext) < delta)
                {
                    widgetsRows = widgetsRowsNext;
                    widgetsCols = widgetsColsNext;
                    delta = Math.Abs(widgets.ActualHeight / widgetsRows - widgets.ActualWidth / widgetsCols);
                    widgetsColsNext++;
                    widgetsRowsNext = (int)Math.Ceiling((double)widgets.Children.Count / widgetsColsNext);
                }

                widgets.Rows = widgetsRows;
                widgets.Columns = widgetsCols;
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (e.NewItems != null)
            {
                foreach (var view in e.NewItems)
                {
                    this.Items.MoveCurrentTo(view);
                }
            }
        }

        private void Widget_Activate(object sender, EventArgs e)
        {
            this.Items.MoveCurrentTo((sender as ScaleBoxCC).Child);
        }

        private void Widget_Close(object sender, EventArgs e)
        {
            var box = sender as ScaleBoxCC;
            var view = box.Child;
            box.Child = null;

            OnRemovePlate(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, view));

            var widgets = Template.FindName(WidgetsPanelTemplatePartName, this) as UniformGrid;
            if (widgets != null)
            {
                widgets.Children.Remove(box);

                //if (widgets.Children.Count == 0)
                //{
                //    var container = Template.FindName(ItemsPanelTemplatePartName, this) as Grid;
                //    container.ColumnDefinitions[0].Width = new GridLength(1);
                //}

                //if (widgets.Children.Count < widgets.Columns * widgets.Rows)
                //{
                //    var actualCellHeight = widgets.ActualHeight / widgets.Columns;
                //    var actualCellWidth = widgets.ActualWidth / widgets.Rows;
                //    var previewActualCellHeight = widgets.ActualHeight / Math.Max(widgets.Rows - 1, 0.001);
                //    var previewActualCellWidth = widgets.ActualWidth / Math.Max(widgets.Columns - 1, 0.001);

                //    if (Math.Abs(previewActualCellHeight - actualCellWidth) < Math.Abs(previewActualCellWidth - actualCellHeight))
                //    {
                //        if (widgets.Rows > 1)
                //            widgets.Rows--;
                //    }
                //    else
                //    {
                //        if (widgets.Columns > 1)
                //            widgets.Columns--;
                //    }
                //}

                Application.Current.Dispatcher.BeginInvoke(new Action(() => this.SetWidgetsArea()), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
        }

        public event EventHandler<NotifyCollectionChangedEventArgs> RemovePlate;
        public void OnRemovePlate(NotifyCollectionChangedEventArgs e)
        {
            if (RemovePlate != null)
            {
                RemovePlate(this, e);
            }
        }
    }
}
