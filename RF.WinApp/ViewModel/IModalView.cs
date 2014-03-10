using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.WinApp.ViewModel
{
    interface IModalView
    {
        bool ModalApplied();
        void ModalClosed();
        string Caption { get; }
        double Width { get; }
        double Height { get; }
        string ApplyCaption { get; }
        string ClearCaption { get; }
    }
}
