using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace RF.WinApp
{
    public class FormsSpace : Decorator
    {
        public static FormsSpace GetSpace(Visual visual)
        {
            return RF.WinApp.Infrastructure.Behaviour.XamlHelper.FindAncestor<FormsSpace>(visual);
        }

        public static void RegisterForm(ActionBlock form)
        {
            var p = FormsSpace.GetSpace(form);
            if (p != null)
            {
                p.AddForm(form);
            }
        }

        private List<ActionBlock> forms = new List<ActionBlock>();

        internal void AddForm(ActionBlock form)
        {
            if (!this.forms.Contains(form))
                this.forms.Add(form);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            foreach (var form in forms)
            {
                GeneralTransform transform = form.TransformToAncestor(this);
                if (transform != null && transform.Inverse != null)
                {
                    Point windowOffset = transform.Inverse.Transform(new Point(0, 0));
                    Point windowLowerRight = windowOffset;
                    windowLowerRight.Offset(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height);
                    var r = new Rect(windowOffset, windowLowerRight);
                    var rg = new RectangleGeometry(r);
                    form.Clip = rg;
                }
            }
        }
    }
}
