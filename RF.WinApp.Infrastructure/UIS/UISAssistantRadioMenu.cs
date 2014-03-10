using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RF.WinApp.Infrastructure.UIS
{
    public class UISAssistantRadioMenu : UISBaseAssistant
    {
        public UISAssistantRadioMenu(UISettingsStoreProvider storeProvider)
            : base(storeProvider)
        {
        }

        public override Type AttendedType
        {
            get { return typeof(RadioMenuHeader); }
        }

        public override void EventsSubscribe(object target, string controlUID)
        {
            var mn = target as RadioMenuHeader;
            mn.RadioChanged += RadioChanged;
        }

        protected void RadioChanged(object sender, RadioMenuEventArgs e)
        {
            var mn = sender as RadioMenuHeader;
            if (mn != null && StoreProvider != null)
            {
                var uid = mn.GetValue(UISettings.ControlUIDProperty) as string;
                var settings = new Dictionary<string, object>();
                settings.Add(RadioMenuHeader.CurrentKeyProperty.Name, e.CurrentKey);
                StoreProvider.PutSettings(uid, settings);
            }
        }
    }
}
