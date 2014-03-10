using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Data;

using RF.WinApp.Infrastructure.CC;

namespace RF.WinApp
{
    public class AdornedForm : ContentControl
    {
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.RegisterAttached("IsShow", typeof(bool), typeof(AdornedForm), new UIPropertyMetadata(false, null, OnCoerceIsShow));
        public static readonly DependencyProperty AdornerLayerHolderProperty
            = DependencyProperty.RegisterAttached("AdornerLayerHolder", typeof(AdornerLayerHolder), typeof(AdornedForm), new UIPropertyMetadata(AdornerLayerHolder.Self, null, OnCoerceAdornerLayerHolder));
        public static readonly DependencyProperty OverlayContentProperty = DependencyProperty.Register("OverlayContent", typeof(ContentControl), typeof(AdornedForm), new FrameworkPropertyMetadata(null));

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
            var form = target as AdornedForm;
            if (form != null)
                form.IsShow = (bool)baseValue;

            return baseValue;
        }

        public static AdornerLayerHolder GetAdornerLayerHolder(DependencyObject target)
        {
            return (AdornerLayerHolder)target.GetValue(AdornerLayerHolderProperty);
        }
        public static void SetAdornerLayerHolder(DependencyObject target, AdornerLayerHolder value)
        {
            target.SetValue(AdornerLayerHolderProperty, value);
        }
        private static object OnCoerceAdornerLayerHolder(DependencyObject target, object baseValue)
        {
            return baseValue;
        }

        public ContentControl OverlayContent
        {
            get { return (ContentControl)GetValue(OverlayContentProperty); }
            set { SetValue(OverlayContentProperty, value); }
        }

        private ControlAdorner _adorner;
        private FrameworkElement _formElement;
        private FrameworkElement FormElement
        {
            get
            {
                //return this.Content as FrameworkElement ?? _formElement;
                return this.OverlayContent;
            }
        }

        private bool _isshow;
        public bool IsShow
        {
            get
            {
                return _isshow;
            }

            set
            {
                System.Diagnostics.Debug.WriteLine(string.Format("IsShow={0}", value));
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

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == "VisibilityProperty")
            {
                if (e.NewValue == (object)Visibility.Collapsed || e.NewValue == (object)Visibility.Hidden)
                {
                    SetIsShow(this, false);
                }
                else
                {
                    SetIsShow(this, true);
                }
            }
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (newContent != null)
            {
                //_formElement = newContent as FrameworkElement;
                //this.Dispatcher.BeginInvoke(new Action(() => { RemoveVisualChild(this.Content as FrameworkElement); }), DispatcherPriority.Background);
            }
        }

        private void Show()
        {
            var holder = GetAdornerLayerHolderElement();



            if (_adorner == null)
            {
                RemoveVisualChild(VisualTreeHelper.GetChild(this, 0) as FrameworkElement);
                var content = FormElement;

                if (double.IsNaN(content.Width))
                {
                    //var wBinding = new Binding("ActualWidth");
                    //wBinding.Source = holder;
                    //BindingOperations.SetBinding(content, FrameworkElement.WidthProperty, wBinding);
                    //content.Width = holder.ActualWidth;
                }

                if (double.IsNaN(content.Height))
                {
                    //var hBinding = new Binding("ActualHeight");
                    //hBinding.Source = holder;
                    //BindingOperations.SetBinding(content, FrameworkElement.HeightProperty, hBinding);
                    //content.Width = holder.ActualHeight;
                }

                try
                {
                    _adorner = new ControlAdorner(this/*, holder*/) { Child = VisualTreeHelper.GetParent(content) as FrameworkElement };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            var adrLayer = AdornerLayer.GetAdornerLayer(this);

            //LogicalTreeHelper

            adrLayer.Remove(_adorner);
            adrLayer.Add(_adorner);
        }

        private void Hide()
        {
            if (_adorner != null)
            {
                AdornerLayer.GetAdornerLayer(this).Remove(_adorner);
            }
        }

        internal FrameworkElement GetAdornerLayerHolderElement()
        {
            FrameworkElement ret = this as FrameworkElement;
            AdornerLayerHolder holder = GetAdornerLayerHolder(this);
            switch (holder)
            {
                case AdornerLayerHolder.Self:
                    break;
                case AdornerLayerHolder.Parent:
                    ret = VisualTreeHelper.GetParent(this) as FrameworkElement;
                    break;
                case AdornerLayerHolder.View:
                    while (ret != null)
                    {
                        var p = VisualTreeHelper.GetParent(ret) as FrameworkElement;
                        if (p != null && p.Name == PlatesCC.MainPanelTemplatePartName)
                            break;
                        ret = p;
                    }
                    break;
                case AdornerLayerHolder.Screen:
                    throw new NotSupportedException("AdornerLayerHolder.Screen");
                case AdornerLayerHolder.App:
                    throw new NotSupportedException("AdornerLayerHolder.App");
            }

            if (ret == null)
                throw new InvalidCastException("Wrong AdornerLayerHolderElement");

            return ret;
        }
    }
}
