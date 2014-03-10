using System;

using RF.WinApp.Infrastructure.Behaviour;
using RF.WinApp.Infrastructure.Models;
using RF.WinApp.Infrastructure.CC;

namespace RF.WinApp.Assets
{
    [ViewExport(RegionName = RegionNames.MenuRegion)]
    public class AssetsMenu : AloneMenuItem
    {
        public AssetsMenu()
            : base("СЧА", "/AssetsView")
        {
        }
    }
}
