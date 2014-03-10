using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace RF.WinApp
{
    public class InCellCurrentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var cell = DataGridHelper.GetCell((DataGridCellInfo)values[1]);
            if (cell != null)
            {
                return cell == values[0];
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsHeaderCurrentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var cell = DataGridHelper.GetCell((DataGridCellInfo)values[1]);
            //var grid = DataGridHelper.GetDataGridFromChild(cell);
            var o = ((DataGridCellInfo)values[1]).Item;
            var h = (DataGridRowHeader)values[0];
            if (cell != null)
            {
                return h.DataContext == o;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal static class DataGridHelper
    {
        internal static DataGridCell GetCell(DataGridCellInfo dataGridCellInfo)
        {
            if (!dataGridCellInfo.IsValid)
            {
                return null;
            }

            var cellContent = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
            if (cellContent != null)
            {
                return (DataGridCell)cellContent.Parent;
            }
            else
            {
                return null;
            }
        }

        internal static DataGrid GetDataGridFromChild(DependencyObject dataGridPart)
        {
            if (dataGridPart == null)
                return null;
            if (VisualTreeHelper.GetParent(dataGridPart) == null)
                return null;
            if (VisualTreeHelper.GetParent(dataGridPart) is DataGrid)
                return (DataGrid)VisualTreeHelper.GetParent(dataGridPart);
            return GetDataGridFromChild(VisualTreeHelper.GetParent(dataGridPart));
        }
    }

    public class SiblingSourceBinding : MarkupExtension
    {
        public SiblingSourceBinding() { }

        public Binding Binding { get; set; }

        public int SiblingIndex { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            //if (provideValueTarget != null)
            //{
            //    this.Binding.Source = LogicalTreeHelper.GetChildren(LogicalTreeHelper.GetParent(provideValueTarget.TargetObject as DependencyObject)).Cast<object>().ElementAt(this.SiblingIndex);
            //}
            //else
            //{
            //    throw new InvalidOperationException("Unable to set sibling binding source.");
            //}

            object o = this.Binding.ProvideValue(serviceProvider);
            return o;
        }
    }

    public class BBidning : MultiBinding
    {
        public string SourceKey { get; set; }

        public BBidning()
        {

            this.Converter = new BMVConverter();
            this.Mode = BindingMode.TwoWay;
            this.UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged;
            this.NotifyOnSourceUpdated = true;
            this.NotifyOnValidationError = true;

            //var page = DispatcherHelper.FindLogicalChildren<Page>(Application.Current.MainWindow).FirstOrDefault();
            //var o = (Binding)Application.Current.FindResource("B1");

            //var w = Application.Current.MainWindow as RF.WinApp.MainWindow;

            //w.MainFrame.Navigated += NavHandler;

            //var o = (Binding)(this..FindResource(new ComponentResourceKey(typeof(AssetsPage), "B"));
            //this.Bindings.Add(o);
        }

        private void NavHandler(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var f = sender as Frame;
            var o = (f.Content as Page).TryFindResource(SourceKey);
            if (o != null && this.Bindings.Count == 0)
                this.Bindings.Add(o as Binding);
        }

        class BMVConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return values[0];
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
            {
                return new object[] { value };
            }
        }
    }

    public class ContentToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var ps = new PathSegmentCollection(4);
            var cp = (FrameworkElement)value;
            //double h = cp.ActualHeight > 10 ? 1.4 * cp.ActualHeight : 10;
            //double w = cp.ActualWidth > 10 ? 1.25 * cp.ActualWidth : 10;
            double w = cp.ActualWidth + 2;
            double h = cp.ActualHeight + 2;

            //ps.Add(new LineSegment(new Point(1, 0.7 * h), true));
            //ps.Add(new BezierSegment(new Point(1, 0.9 * h), new Point(0.1 * h, h), new Point(0.3 * h, h), true));

            ps.Add(new LineSegment(new Point(1, h - 4), true));
            ps.Add(new ArcSegment(new Point(5, h), new Size(5, 5), 0, false, SweepDirection.Counterclockwise, true));

            ps.Add(new LineSegment(new Point(w, h), true));
            //ps.Add(new BezierSegment(new Point(w + 0.6 * h, h), new Point(w + h, 0), new Point(w + h * 1.3, 0), true));
            //ps.Add(new BezierSegment(new Point(w + 0.4 * h, h), new Point(w + 0.9 * h, 0), new Point(w + h * 1.2, 0), true));
            ps.Add(new ArcSegment(new Point(w + 4, h - 4), new Size(5, 5), 0, false, SweepDirection.Counterclockwise, true));
            ps.Add(new LineSegment(new Point(w + 10, 3), true));
            ps.Add(new ArcSegment(new Point(w + 14, -1), new Size(5, 5), 0, false, SweepDirection.Clockwise, true));
            return ps;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
