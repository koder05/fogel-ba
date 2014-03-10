using System;
using System.Collections.Generic;
using System.Windows.Controls;

using System.ComponentModel.Composition;

using RF.BL.Model.Enums;

namespace RF.WinApp.Assets.Views
{
    [Export("GovernorView")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class GovernorView : UserControl
    {
        public GovernorView()
        {
            InitializeComponent();
        }

        public IEnumerable<EnumViewModel<LawFormType>> LawForms
        {
            get
            {
                return new EnumViewModel<LawFormType>().GetList();
            }
        }
    }
}
