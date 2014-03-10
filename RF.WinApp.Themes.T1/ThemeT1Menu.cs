using System;
using System.ComponentModel.Composition;

using RF.WinApp.Infrastructure.Behaviour;
using RF.WinApp.Infrastructure.Models;

namespace RF.WinApp.Themes.T1
{
    [ViewExport(RegionName = RegionNames.MenuThemesRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ThemeT1Menu : RadioMenuItem 
    {
        public ThemeT1Menu()
            : base("Flat T1", "RF.WinApp.Themes.T1")
        {
        }
    }
}
