using System;

namespace RF.Common.UI
{
    public interface IUIErrorOverdoorBehavior
    {
        void ShowError(Exception ex);
    }
}
