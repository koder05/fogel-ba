using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace RF.WinApp.Geo.Views
{
    [Export("GeoaddrView")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class GeoaddrView : UserControl
    {
        public GeoaddrView()
        {
            InitializeComponent();
        }

                private void Parse(object sender, RoutedEventArgs e)
        {
            var dobj = (sender as Control).DataContext as JIT.DataObj;
            var addr = dobj.Model as RF.Geo.BL.Addr;
            //addr.PropertyChanged += AddrPropChanged;
            addr.Parse();
        }

        private void AddrPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TrueCode")
            {
                if (this.AddrCRUD.MoveForward())
                {
                    Parse(btnParse, null);
                }
            }
        }

        private CancellationTokenSource ctsAutoParsrTask = null;

        private void AutoParseStartStop(object sender, RoutedEventArgs e)
        {
            if (ctsAutoParsrTask != null && ctsAutoParsrTask.IsCancellationRequested == false)
            {
                ctsAutoParsrTask.Cancel();
            }
            else
            {
                bAddrCRUDDisabler.Visibility = Visibility.Visible;
                ctsAutoParsrTask = new CancellationTokenSource();
                var ct = ctsAutoParsrTask.Token;

                Task.Factory.StartNew(() => AutoParse(ct), ct)
                .ContinueWith((t) => 
                    {
                        if (!t.IsCanceled)
                            ctsAutoParsrTask.Cancel();
                        if (t.IsFaulted)
                            Application.Current.Dispatcher.BeginInvoke(new Action(() => { throw t.Exception.InnerException; }), DispatcherPriority.Send);
                    } //, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously
                    , TaskScheduler.FromCurrentSynchronizationContext()
                    )
                ;
                
                ct.Register(() => bAddrCRUDDisabler.Visibility = Visibility.Collapsed);
            }
        }

        private void AutoParse(CancellationToken ct)
        {
            bool condition = true;
            int i = 0;
            do
            {
                var obj = AddrCRUD.Dispatcher.Invoke(new Func<object>(() => AddrCRUD.SelectedItem), DispatcherPriority.Background) as RF.WinApp.JIT.DataObj;
                var dobj = obj.Model as RF.Geo.BL.Addr;

                if (dobj != null)
                {
                    gimgWait.Dispatcher.Invoke(new Action(() => gimgWait.Visibility = Visibility.Visible), DispatcherPriority.Background);
                    try
                    {
                        var found = dobj.Parse();
                        if (found != null)
                        {
                            AddrCRUD.DataViewProvider.Update(dobj);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        gimgWait.Dispatcher.Invoke(new Action(() => gimgWait.Visibility = Visibility.Hidden), DispatcherPriority.Background);
                    }
                }
                condition = (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => AddrCRUD.MoveForward()), DispatcherPriority.ApplicationIdle);
                i++;

                if (i > 100)
                {
                    GC.Collect();
                    i = 0;
                }
            }
            while (condition && !ct.IsCancellationRequested);
        }
    }
}
