using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

using RF.LinqExt;
using RF.WinApp.Infrastructure.Behaviour;
using RF.WinApp.JIT;

namespace RF.WinApp
{
    public class FilterBlockCC : ActionBlock
    {
        static FilterBlockCC()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterBlockCC), new FrameworkPropertyMetadata(typeof(FilterBlockCC)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //need process other events to complete all filter controls
            Application.Current.Dispatcher.BeginInvoke(new Action(() => OnFilterApplied(new FilterBlockEventArgs(GetFilters()))), System.Windows.Threading.DispatcherPriority.Background);
        }

        protected override void Apply(object sender, ExecutedRoutedEventArgs e)
        {
            OnFilterApplied(new FilterBlockEventArgs(GetFilters()));
        }

        protected override void Clear(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (FilterCC f in DispatcherHelper.FindVisualChildren<FilterCC>(this))
            {
                f.Value = null;
                f.DataContext = null;
            }
        }

        public Type FilteredModelType { get; set; }

        private FilterParameterCollection GetFilters()
        {
            Type modelType = this.FilteredModelType;
            if (modelType == null)
            {
                var dobj = this.DataContext as DataObj;
                if (dobj != null)
                {
                    modelType = dobj.Model.GetType();
                }
            }

            if (modelType != null)
            {
                FilterParameterCollection fc = new FilterParameterCollection(modelType);
                foreach (FilterCC f in DispatcherHelper.FindVisualChildren<FilterCC>(this).Where(f => f.IsEnabled))
                {
                    if (f.Value != null)
                    {
                        if (f.Value.GetType() == typeof(string))
                        {
                            string s = f.Value as string;
                            if (string.IsNullOrEmpty(s) == false)
                            {
                                Type targtType = null;
                                var fiedInfo = modelType.GetProperty(f.FieldName);
                                if (fiedInfo != null)
                                {
                                    targtType = fiedInfo.PropertyType;
                                    var val = Convert.ChangeType(f.Value, targtType);
                                    OperatorType op = f.OperatorType;
                                    if (op == OperatorType.Equals && targtType == typeof(string))
                                    {
                                        if (s.Contains("?") || s.Contains("*"))
                                        {
                                            op = OperatorType.Like;
                                        }
                                    }
                                    fc.Add(f.FieldName, val, op);
                                }
                            }
                        }
                        else
                        {
                            fc.Add(f.FieldName, f.Value, f.OperatorType);
                        }
                    }
                    else if (f.OperatorType == OperatorType.IsNull || f.OperatorType == OperatorType.IsNotNull)
                    {
                        fc.Add(f.FieldName, f.Value, f.OperatorType);
                    }
                }
                return fc;
            }

            return null;
        }

        public event EventHandler<FilterBlockEventArgs> FilterApplied;
        public void OnFilterApplied(FilterBlockEventArgs e)
        {
            if (FilterApplied != null)
            {
                FilterApplied(this, e);
            }
        }
    }

    public class FilterBlockEventArgs : EventArgs
    {
        public FilterParameterCollection Filters { get; private set; }
        public FilterBlockEventArgs(FilterParameterCollection fc)
        {
            this.Filters = fc;
        }
    }
}
