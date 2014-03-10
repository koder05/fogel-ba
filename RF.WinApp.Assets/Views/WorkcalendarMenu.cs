using System;

using RF.WinApp.Infrastructure.Behaviour;
using RF.WinApp.Infrastructure.Models;
using RF.WinApp.Infrastructure.CC;

namespace RF.WinApp.Assets
{
    [ViewExport(RegionName = RegionNames.MenuRegion)]
    public class WorkcalendarMenu : AloneMenuItem
    {
        public WorkcalendarMenu()
            : base("Календарь", "/WorkcalendarView")
        {
        }
    }
}
