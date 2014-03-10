using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Practices.Prism.Regions;

using RF.WinApp.Infrastructure.Behaviour;
using RF.WinApp.ViewModel;

namespace RF.WinApp.Views
{
    //[ViewExport(RegionName = "MenuRegion")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ViewSortHint("1")]
    public partial class HomeMenu : MenuItem
    {
        public HomeMenu()
        {
            InitializeComponent();
        }

        [Import]
        public HomeNavigatorModel NavigationViewModel
        {
            get { return this.DataContext as HomeNavigatorModel; }
            set { this.DataContext = value; }
        }
    }
}
