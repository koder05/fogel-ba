using System;

using RF.WinApp.Infrastructure.Behaviour;
using RF.WinApp.Infrastructure.Models;
using RF.WinApp.Infrastructure.CC;

namespace RF.WinApp.Assets
{
    [ViewExport(RegionName = RegionNames.MenuRegion)]
    public class GovernorMenu : AloneMenuItem
    {
        public GovernorMenu()
            : base("УК", "/GovernorView")
        {
        }
    }
}
