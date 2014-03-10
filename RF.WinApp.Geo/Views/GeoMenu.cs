using System;
using Microsoft.Practices.Prism.Regions;

using RF.WinApp.Infrastructure.Behaviour;
using RF.WinApp.Infrastructure.Models;
using RF.WinApp.Infrastructure.CC;

namespace RF.WinApp.Geo
{
    [ViewExport(RegionName = RegionNames.MenuRegion)]
    [ViewSortHint("99")]
    public class GeoMenu : AloneMenuItem
    {
        public GeoMenu()
            : base("Адреса", "/GeoaddrView")
        {
        }
    }
}

