using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Commands;

using RF.WinApp.Infrastructure.Models;

namespace RF.WinApp.Infrastructure.CC
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ViewSortHint("9")]
    public class AloneMenuItem : SplitButton
    {
        private string _view;

        public AloneMenuItem(string header, string view)
        {
            this.Header = header;
            this._view = view;
            this.Command = new DelegateCommand(OnShowExecuted);

            this.ButtonMenuItemsSource.Add(new MenuItem() { Header = "Закладка", ToolTip = "Открыть в новой закладке"
                , Icon = new Image() { Source = new BitmapImage(new Uri("pack://application:,,,/RF.WinApp;component/Img/tab_new.png")), Width = 18, Height = 18 }
                , Command = new DelegateCommand(() => { TabsPlatesCC.SetAddingOrder(true);  this.Command.Execute(this.CommandParameter); }) });
            this.ButtonMenuItemsSource.Add(new MenuItem() { Header = "Плитка", ToolTip = "Открыть в текущей закладке"
                , Icon = new Image() { Source = new BitmapImage(new Uri("pack://application:,,,/RF.WinApp;component/Img/application_view_tile.png")), Width = 18, Height = 18 }
                , Command = new DelegateCommand(() => { TabsPlatesCC.SetAddingOrder(false);  this.Command.Execute(this.CommandParameter); }) });
            //var style = Application.Current.TryFindResource(typeof(SplitButton)) as Style;
            //this.SetValue(FrameworkElement.StyleProperty, style);
        }

        [Import]
        public IRegionManager regionManager;

        private void OnShowExecuted()
        {
            Uri viewNav = new Uri(_view, UriKind.Relative);
            regionManager.RequestNavigate(RegionNames.WorkspaceRegion, viewNav);
        }
    }
}
