using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.WinApp.Infrastructure.UIS
{
    public interface IUISettingsTypeAssistant
    {
        Type AttendedType { get; }
        void EventsSubscribe(object target, string controlUID);
        void AttendInstance(object target, string controlUID);
    }
}
