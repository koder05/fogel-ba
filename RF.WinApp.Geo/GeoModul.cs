using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace RF.WinApp.Geo
{
    [ModuleExport(typeof(GeoModule))]
    public class GeoModule : IModule
    {
        public void Initialize()
        {
        }
    }
}
