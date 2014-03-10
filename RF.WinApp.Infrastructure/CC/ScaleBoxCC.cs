using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RF.WinApp
{
    [TemplatePart(Name = ViewboxTemplatePartName, Type = typeof(Viewbox))]
    public class ScaleBoxCC : Control
    {
        private const string ViewboxTemplatePartName = "PART_Viewbox";

        private Viewbox _viewbox = null;
        private UIElement _child = null;

        static ScaleBoxCC()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScaleBoxCC), new FrameworkPropertyMetadata(typeof(ScaleBoxCC)));
            CloseAction = new RoutedCommand("CloseAction", typeof(ScaleBoxCC));
        }

        public ScaleBoxCC(FrameworkElement child)
        {
            if (child != null)
            {
                child.Height = child.ActualHeight;
                child.Width = child.ActualWidth;
                _child = child;
                this.SetValue(ViewName.TitleProperty, _child.GetValue(ViewName.TitleProperty));
            }
        }

        public ScaleBoxCC()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _viewbox = Template.FindName(ViewboxTemplatePartName, this) as Viewbox;

            if (_child != null && _viewbox != null)
            {
                _viewbox.GotFocus += (sender, e) => OnActivate(EventArgs.Empty);
                if (_viewbox != null)
                {
                    _viewbox.Child = _child;
                    _child = null;
                }
            }

            CommandBindings.Add(new CommandBinding(CloseAction, Close));
        }

        public UIElement Child
        {
            get
            {
                return _viewbox != null ? _viewbox.Child : _child;
            }
            set
            {
                if (_viewbox != null)
                    _viewbox.Child = value;
                else
                    _child = value;
            }
        }

        public event EventHandler Closed;
        public void OnClosed(EventArgs e)
        {
            if (Closed != null)
            {
                Closed(this, e);
            }
        }

        public event EventHandler Activate;
        public void OnActivate(EventArgs e)
        {
            if (Activate != null)
            {
                Activate(this, e);
            }
        }

        public static RoutedCommand CloseAction { get; private set; }
        public virtual void Close(object sender, ExecutedRoutedEventArgs e)
        {
            OnClosed(EventArgs.Empty);
        }
    }
}
