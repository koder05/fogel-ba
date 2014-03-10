using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;

using RF.WinApp.ViewModel;
using RF.WinApp.Infrastructure.Behaviour;

namespace RF.WinApp.Views
{
    [Export("LogonView")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LogonView : UserControl, IModalView
    {
        private AutoResetEvent _logonFormCompleteEvent;

        public LogonView(AutoResetEvent logonFormCompleteEvent)
        {
            InitializeComponent();
            _logonFormCompleteEvent = logonFormCompleteEvent;
        }

        public void AttachModel(LogonModel model)
        {
            this.DataContext = model;
        }

        public bool ModalApplied()
        {
            if (this.ValidateAll() == false)
                return false;

            var model = this.DataContext as LogonModel;
            model.Password = tbPsw.Password;
            model.IsOk = true;
            return true;
        }

        public void ModalClosed()
        {
            _logonFormCompleteEvent.Set();
        }

        public string Caption
        {
            get { return "Вход на сервер"; }
        }

        public string ApplyCaption
        {
            get { return "OK"; }
        }

        public string ClearCaption
        {
            get { return "Отмена"; }
        }
    }
}
