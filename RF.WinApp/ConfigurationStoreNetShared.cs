using System.Reflection;
using System.Configuration;

namespace Microsoft.Practices.Prism.Modularity
{
    /// <summary>
    /// Enable launch over local net w/o error (ConfigurationManager.GetSection())
    /// </summary>
    public class ConfigurationStoreNetShared : IConfigurationStore
    {
        /// <summary>
        /// Gets the module configuration data.
        /// </summary>
        /// <returns>A <see cref="ModulesConfigurationSection"/> instance.</returns>
        public ModulesConfigurationSection RetrieveModuleConfigurationSection()
        {
            var section = LoadConfig();
            return section as ModulesConfigurationSection;
        }

        private ConfigurationSection LoadConfig()
        {
            var path = Assembly.GetExecutingAssembly().Location + ".config";
            var cfgFileMap = new ExeConfigurationFileMap() { ExeConfigFilename = path };
            var cfg = ConfigurationManager.OpenMappedExeConfiguration(cfgFileMap, ConfigurationUserLevel.None);
            return cfg.GetSection("modules");
        }
    }
}
