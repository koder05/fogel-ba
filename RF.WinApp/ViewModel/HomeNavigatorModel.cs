using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;

namespace RF.WinApp.ViewModel
{
    [Export(typeof(HomeNavigatorModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeNavigatorModel : NotificationObject
    {
        public IEnumerable<object> MenuObjects { get; set; }

        [Import]
        public IRegionManager regionManager;


        [ImportingConstructor]
        public HomeNavigatorModel()
        {
            MenuObjects = new object[]
                              {
                                 new {  Name = "УК", View = "/GovernorView2",  ActivateCommand = new DelegateCommand<string>(OnShowExecuted) },
                                  new { Name = "Календарь",  View = "/WorkCalendarView", ActivateCommand = new DelegateCommand<string>(OnShowExecuted)},
                                     new {Name = "СЧА",View = "/AssetsView", ActivateCommand = new DelegateCommand<string>(OnShowExecuted) }
                              };
        }

        private void OnShowExecuted(string view)
        {

            Uri viewNav = new Uri(view, UriKind.Relative);
            regionManager.RequestNavigate("WorkspaceRegion", viewNav);
        }
    }
}
