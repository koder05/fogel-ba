using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace RF.WinApp.Themes.T1
{
    [ModuleExport(typeof(ThemeT1Module))]
    public class ThemeT1Module : IModule
    {
        public void Initialize()
        {
        }
    }
}
