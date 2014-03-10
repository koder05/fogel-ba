using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RF.WinApp.Infrastructure.UIS
{
    public interface IUISettingsStoreProviderAgent
    {
        Dictionary<string, object> GetSettings(string controlUid);
        void PutSettings(string controlUid, Dictionary<string, object> settings);
    }
}
