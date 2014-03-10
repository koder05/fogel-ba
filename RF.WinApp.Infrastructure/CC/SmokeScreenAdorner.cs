using System;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;

namespace RF.WinApp.Infrastructure.CC
{
    public class SmokeScreenAdorner : Adorner
    {
        private FrameworkElement _scope;
        public SmokeScreenAdorner(UIElement adornedElement, FrameworkElement scope)
            : base(adornedElement)
        {
            if (scope == null)
                throw new ArgumentException("can't get main window");

            _scope = scope;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //drawingContext.DrawRectangle(new SolidColorBrush() { Color = Color.FromArgb(0xff, 0x68, 0x8c, 0xaf), Opacity = 0.3 }, null, WindowRect());
            drawingContext.DrawRectangle(new SolidColorBrush() { Color = Color.FromArgb(0xff, 0x68, 0x8c, 0xaf), Opacity = 0.3 }, null, new Rect(new Point(-5000, -5000), new Point(5000, 5000)));
            base.OnRender(drawingContext);
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            GeometryGroup grp = new GeometryGroup();
            var r1 = WindowRect();
            var r2 = new Rect(new Size(Math.Min(layoutSlotSize.Width, r1.Right), Math.Min(layoutSlotSize.Height, r1.Bottom)));


            var space = FormsSpace.GetSpace(this.AdornedElement);
            if (this.AdornedElement.Clip != null)
            {
                var rg = this.AdornedElement.Clip;
                r2 = new Rect(new Size(Math.Min(layoutSlotSize.Width, rg.Bounds.Right), Math.Min(layoutSlotSize.Height, rg.Bounds.Bottom)));
            }

            var rg1 = new RectangleGeometry(r1);
            var rg2 = new RectangleGeometry(r2);
            grp.Children.Add(rg1);
            grp.Children.Add(rg2);
            
            return grp;
        }

        private Rect WindowRect()
        {
            Point windowOffset = new Point(0, 0);
            GeneralTransform transformToAncestor = this.AdornedElement.TransformToAncestor(_scope);
            
            if (transformToAncestor == null || transformToAncestor.Inverse == null)
                throw new ArgumentException("no transform to window");
                
            windowOffset = transformToAncestor.Inverse.Transform(new Point(0, 0));
            Point windowLowerRight = windowOffset;
            windowLowerRight.Offset(_scope.ActualWidth, _scope.ActualHeight);
            return new Rect(windowOffset, windowLowerRight);
        }
    }
}
