using System;
using System.ComponentModel;

namespace RF.BL.Model.Enums
{
    public enum InsuranceType : byte
    {
        [Description("ОПС")]
        OPS = 1,

        [Description("НПО")]
        NPO = 2
    }
}
