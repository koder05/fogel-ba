using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

using System.Windows.Media;

using RF.WinApp.Infrastructure.CC;

namespace RF.WinApp
{
    public class AdornedPopup : ContentControl
    {
        public static readonly DependencyProperty IsKeeperElementProperty = DependencyProperty.RegisterAttached("IsKeeperElement", typeof(bool), typeof(AdornedPopup), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.RegisterAttached("IsShow", typeof(bool), typeof(AdornedPopup), new UIPropertyMetadata(false, null, OnCoerceIsShow));
        public static readonly DependencyProperty ErrProperty = DependencyProperty.RegisterAttached("Err", typeof(ValidationError), typeof(AdornedPopup), new UIPropertyMetadata(null, null, OnCoerceErr));

        public static bool GetIsKeeperElement(DependencyObject target)
        {
            return (bool)target.GetValue(IsKeeperElementProperty);
        }
        public static void SetIsKeeperElement(DependencyObject target, bool value)
        {
            target.SetValue(IsKeeperElementProperty, value);
        }

        public static bool GetIsShow(DependencyObject target)
        {
            return (bool)target.GetValue(IsShowProperty);
        }
        public static void SetIsShow(DependencyObject target, bool value)
        {
            target.SetValue(IsShowProperty, value);
        }
        private static object OnCoerceIsShow(DependencyObject target, object baseValue)
        {
            var popup = target as AdornedPopup;
            if (popup != null)
                popup.IsShow = (bool)baseValue;

            return baseValue;
        }

        public static ValidationError GetErr(DependencyObject target)
        {
            return (ValidationError)target.GetValue(ErrProperty);
        }
        public static void SetErr(DependencyObject target, ValidationError value)
        {
            target.SetValue(ErrProperty, value);
        }
        private static object OnCoerceErr(DependencyObject target, object baseValue)
        {
            var err = baseValue as ValidationError;
            var element = target as FrameworkElement;
            if (element != null)
            {
                if (element.BindingGroup.Name != "OnCoerceErrBindingGroup")
                {
                    element.BindingGroup = new System.Windows.Data.BindingGroup() { Name = "OnCoerceErrBindingGroup" };
                }

                element.BindingGroup.ValidationRules.Clear();
                element.BindingGroup.ValidationRules.Add(new RF.WinApp.Infrastructure.Behaviour.BubbleErrorValidationRule(err));
                element.BindingGroup.ValidateWithoutUpdate();
            }

            return baseValue;
        }

        private ControlAdorner _adorner;

        private FrameworkElement content;

        private bool _isshow;
        public bool IsShow
        {
            get
            {
                return _isshow;
            }

            set
            {
                if (value)
                {
                    Show();
                }
                else
                {
                    Hide();
                }

                _isshow = value;
            }
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (newContent != null)
            {
                content = newContent as FrameworkElement;
                //this.RemoveVisualChild(content); this.RemoveLogicalChild(content);
                this.Dispatcher.BeginInvoke(new Action(() => { this.Content = null; content.Visibility = System.Windows.Visibility.Visible; }), DispatcherPriority.Background);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == "IsVisible")
            {
                if ((bool)e.NewValue == false)
                {
                    SetIsShow(this, false);
                }
            }
        }

        private void Show()
        {
            if (_adorner == null)
            {
                try
                {
                    _adorner = new ControlAdorner(this) { Child = content };
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //System.Diagnostics.Debug.WriteLine("Show() new _adorner");
            }

            AdornerLayer.GetAdornerLayer(this).Remove(_adorner);
            AdornerLayer.GetAdornerLayer(this).Add(_adorner);
            GetMouseCapture(content);
        }

        private void Hide()
        {
            System.Diagnostics.Debug.WriteLine("Hide()");

            RemoveMouseCapture(content);
            RemoveMouseCapture(_keeper);
            if (_adorner != null)
            {
                //System.Diagnostics.Debug.WriteLine("Hide() exists _adorner");
                AdornerLayer.GetAdornerLayer(this).Remove(_adorner);
                _adorner.Child = null;
                _adorner = null;
                content.Visibility = System.Windows.Visibility.Collapsed;
                this.Content = content;
            }
        }

        private void GetMouseCapture(FrameworkElement el)
        {
            if (el != null && Mouse.Captured != el)
            {
                el.IsMouseCaptureWithinChanged -= _IsMouseCapturedChanged;
                Mouse.Capture(el, CaptureMode.SubTree);
                Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(el, PreviewMouseDownOutsideCapturedElement);
                Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(el, PreviewMouseDownOutsideCapturedElement);
                el.IsMouseCaptureWithinChanged += _IsMouseCapturedChanged;
            }
        }

        private void RemoveMouseCapture(FrameworkElement el)
        {
            if (el != null)
            {
                el.IsMouseCaptureWithinChanged -= _IsMouseCapturedChanged;
                Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(el, PreviewMouseDownOutsideCapturedElement);
                Mouse.Capture(null);
            }
        }

        private void _IsMouseCapturedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var el = sender as FrameworkElement;
            if ((bool)e.OldValue == true && (bool)e.NewValue == false)
            {
                GetMouseCapture(el);
            }
        }

        private HitTestResultBehavior HitTestResult(HitTestResult result)
        {
            return HitTestResultBehavior.Continue;
        }

        private HitTestFilterBehavior HitTestFilter(DependencyObject o)
        {
            if ((bool)o.GetValue(IsKeeperElementProperty))
            {
                _keeper = o as FrameworkElement;
                //System.Diagnostics.Debug.WriteLine(string.Format("VisualHit in filter type of {0}, name {1}", _keeper.GetType(), _keeper.Name ));
                return HitTestFilterBehavior.Stop;
            }

            return HitTestFilterBehavior.Continue;
        }

        private FrameworkElement _keeper;

        private void PreviewMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("PreviewMouseDownOutsideCapturedElement type of {0}", sender.GetType()));
            bool isInside = IsMouseClickWithin(content, e.MouseDevice.GetPosition(content));

            RemoveMouseCapture(_keeper);
            _keeper = null;

            if (isInside)
            {
                GetMouseCapture(content);
            }
            else
            {
                FrameworkElement hitElement = RF.WinApp.Infrastructure.Behaviour.XamlHelper.FindAncestor<UserControl>(this);
                if (hitElement == null)
                    hitElement = this;
                var pt = e.MouseDevice.GetPosition(hitElement);
                VisualTreeHelper.HitTest(hitElement, HitTestFilter, HitTestResult, new PointHitTestParameters(pt));

                if (_keeper != null)
                {
                    RemoveMouseCapture(content);
                    GetMouseCapture(_keeper);
                }
                else
                {
                    SetIsShow(this, false);
                    e.Handled = true;
                }
            }
        }

        private bool IsMouseClickWithin(FrameworkElement element, Point point)
        {
            if (element != null)
                return element.ActualWidth >= point.X && point.X >= 0 && element.ActualHeight >= point.Y && point.Y >= 0;

            return false;
        }
    }
}
