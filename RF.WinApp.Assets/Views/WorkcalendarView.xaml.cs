using System;
using System.Windows.Controls;

using System.ComponentModel.Composition;

namespace RF.WinApp.Assets.Views
{
    [Export("WorkcalendarView")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class WorkcalendarView : UserControl
    {
        public WorkcalendarView()
        {
            InitializeComponent();
        }

        public static DateTime DefaultFromDate = DateTime.Today.AddYears(-3);
    }
}
