using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RF.WinApp.Infrastructure.UIS
{
    public class UISettingsStoreProvider
    {
        private IUISettingsStoreProviderAgent _agent;

        public UISettingsStoreProvider()
        {
        }

        public UISettingsStoreProvider(IUISettingsStoreProviderAgent agent)
        {
            _agent = agent;
        }

        public Dictionary<string, object> GetSettings(string controlUid)
        {
            if (_agent != null)
                return _agent.GetSettings(controlUid);
            
            return null;
        }

        public void PutSettings(string controlUid, Dictionary<string, object> settings)
        {
            if (_agent != null)
                _agent.PutSettings(controlUid, settings);
        }
    }
}
