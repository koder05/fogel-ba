using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using RF.Common.Security;
using RF.WinApp.Views;
using RF.WinApp.ViewModel;

using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;

namespace RF.WinApp
{
    /// <summary>
    /// multithread unsafe class
    /// </summary>
    [Export(typeof(ILogonPage))]
    public class LogonProvider : ILogonPage
    {
        private AutoResetEvent logonFormCompleteEvent = new AutoResetEvent(true);

        public LogonCreds GetLogin(Exception showReason)
        {
            var model = new LogonModel();

            logonFormCompleteEvent.Reset();

            ShowLogonFormModal(model);

            logonFormCompleteEvent.WaitOne();

            return new LogonCreds() { Name = model.Login, Psw = model.Password, IsCanceled = !model.IsOk, IsSuccessful = model.IsOk };
        }

        public void ShowLogonFormModal(LogonModel model)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => { _showLogonFormModal(model); }), DispatcherPriority.Background);
        }

        /// <summary>
        /// delegate to launch in the main application thread
        /// </summary>
        private void _showLogonFormModal(LogonModel model)
        {
            LogonView view = new LogonView(logonFormCompleteEvent);
            view.AttachModel(model);
            _regionManager.Regions["ModalRegion"].Add(view);
        }

        [Import]
        public IRegionManager _regionManager;
    }
}
