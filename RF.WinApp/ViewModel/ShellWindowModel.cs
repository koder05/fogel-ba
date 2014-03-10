using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace RF.WinApp.ViewModel
{
    [Export(typeof(ShellWindowModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ShellWindowModel
    {
        [ImportingConstructor]
        public ShellWindowModel()
        {
            ExitCommand = new DelegateCommand(OnExited);
            Status = "Shell Alone";
        }

        public ICommand ExitCommand { get; private set; }

        private void OnExited()
        {
            Application.Current.Shutdown();
        }

        public string Status { get; set; }


    }
}
