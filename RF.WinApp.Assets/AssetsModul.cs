
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace RF.WinApp.Assets
{
    [ModuleExport(typeof(AssetsModule))]
    public class AssetsModule : IModule
    {
        public void Initialize()
        {
        }
    }
}
