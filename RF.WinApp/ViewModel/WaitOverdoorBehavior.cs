using System.Windows;

using RF.Common.UI;

namespace RF.WinApp.ViewModel
{
    internal class WaitOverdoorBehavior : IUIWaitOverdoorBehavior
    {
        public void OverdoorOn()
        {
            (Application.Current.MainWindow as ShellWindow).WaitOverDoorOn();
        }

        public void OverdoorOff()
        {
            (Application.Current.MainWindow as ShellWindow).WaitOverDoorOff();
        }
    }
}
