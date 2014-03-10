using System;
using System.Collections.Generic;
using System.Windows.Controls;

using RF.WinApp.Infrastructure;
using RF.BL.Model.Enums;

namespace RF.WinApp
{
    public partial class GovernorLookup : UserControl, ICrudLookupMode
    {
        public GovernorLookup()
        {
            InitializeComponent();
        }

        public CrudCC LookupControl
        {
            get { return TemplatedCrudCC; }
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
