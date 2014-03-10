using System;
using System.Windows;

using RF.Common.UI;

namespace RF.WinApp.ViewModel
{
    public class ErrorOverdoorBehavior : IUIErrorOverdoorBehavior
    {
        private bool isSignaled = true;

        public void ShowError(Exception ex)
        {
            if (!isSignaled)
                return;

            isSignaled = false;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            //string errorMessage = string.Format("Произошла ошибка.\n\nError:{0}\n\nСтоит ли продолжать?", ex.Message + (ex.InnerException != null ? "\n" + ex.InnerException.Message : null));

            string errorMessage = string.Format("Произошла ошибка.\n\nError:{0}\n\nStackTrace:{1}\n\nСтоит ли продолжать?", ex.Message, ex.StackTrace.Substring(0, 2000) );

            if (MessageBox.Show(Application.Current.MainWindow, errorMessage, "Application Error", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.No)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                //new Thread(delegate() { Dispatcher.BeginInvoke((Action)delegate() { Application.Current.Shutdown(); }); }).Start();
            }

            isSignaled = true;
        }
    }
}
