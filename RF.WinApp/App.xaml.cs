using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Threading.Tasks;

using RF.Common.DI;
using RF.Common.Transactions;
using Ms.Unity._2;
using RF.WinApp.ViewModel;
using RF.Common.UI;
using RF.WinApp.Infrastructure.UIS;

namespace RF.WinApp
{
    public partial class App : Application
    {
        public App()
        {
            UIErrorOverdoor.SetBehavior(new ErrorOverdoorBehavior());
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            IContainerWrapper ioc = new UnityContainerWrapper();
            IoC.InitializeWith(ioc);
            Transactions.Service = IoC.Resolve<TransactionService>();

            foreach (var uiAssistant in IoC.ResolveAll<IUISettingsTypeAssistant>())
                UISettings.RegisterTypeAssistant(uiAssistant);

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs e) =>
            {
                e.SetObserved();
                UIErrorOverdoor.Show(e.Exception);
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new Prism4MefBootstapper();
            bootstrapper.Run();
            UIWaitOverdoor.SetBehavior(new WaitOverdoorBehavior());
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG   // In debug mode do not custom-handle the exception, let Visual Studio handle it
            e.Handled = false;
            ShowUnhandeledException(e); 
#else
            ShowUnhandeledException(e);
#endif
        }

        void ShowUnhandeledException(System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            UIErrorOverdoor.Show(e.Exception);
        }
    }
}
