using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RF.WinApp
{
    public class ActionBlock : ContentControl
    {
        public static readonly DependencyProperty BlockCaptionProperty;
        public static readonly DependencyProperty ApplyCaptionProperty;
        public static readonly DependencyProperty ClearCaptionProperty;
        public static readonly DependencyProperty ModalScopeElementProperty;

        static ActionBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActionBlock), new FrameworkPropertyMetadata(typeof(ActionBlock)));

            BlockCaptionProperty = DependencyProperty.Register("BlockCaption", typeof(String), typeof(ActionBlock), new UIPropertyMetadata("Block"));
            ApplyCaptionProperty = DependencyProperty.Register("ApplyCaption", typeof(String), typeof(ActionBlock), new UIPropertyMetadata("Apply"));
            ClearCaptionProperty = DependencyProperty.Register("ClearCaption", typeof(String), typeof(ActionBlock), new UIPropertyMetadata("Clear"));
            ModalScopeElementProperty = DependencyProperty.Register("ModalScopeElement", typeof(FrameworkElement), typeof(ActionBlock), new UIPropertyMetadata(null, null, OnCoerceModalScopeElement));

            ApplyAction = new RoutedCommand("ApplyAction", typeof(ActionBlock));
            ClearAction = new RoutedCommand("ClearAction", typeof(ActionBlock));
            CloseAction = new RoutedCommand("CloseAction", typeof(ActionBlock));
        }

        public readonly static DependencyProperty IsOpenedProperty = DependencyProperty.RegisterAttached("IsOpened", typeof(bool), typeof(ActionBlock), new UIPropertyMetadata(false));
        public static object GetIsOpened(DependencyObject target)
        {
            return target.GetValue(IsOpenedProperty);
        }
        public static void SetIsOpened(DependencyObject target, string value)
        {
            target.SetValue(IsOpenedProperty, value);
        }

        public readonly static DependencyProperty IsShadedProperty = DependencyProperty.RegisterAttached("IsShaded", typeof(bool), typeof(ActionBlock), new UIPropertyMetadata(false));
        public static object GetIsShaded(DependencyObject target)
        {
            return target.GetValue(IsShadedProperty);
        }
        public static void SetIsShaded(DependencyObject target, string value)
        {
            target.SetValue(IsShadedProperty, value);
        }

        private static object OnCoerceModalScopeElement(DependencyObject target, object baseValue)
        {
            var form = target as AdornedForm;
            if (form != null)
                form.IsShow = (bool)baseValue;

            return baseValue;
        }

        public ActionBlock()
        {
            CommandBindings.Add(new CommandBinding(ApplyAction, Apply));
            CommandBindings.Add(new CommandBinding(ClearAction, Clear));
            CommandBindings.Add(new CommandBinding(CloseAction, Close));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            FormsSpace.RegisterForm(this);
        }

        public bool IsOpened
        {
            get { return this.Visibility == System.Windows.Visibility.Visible; }
        }

        public bool IsNotEmpty
        {
            get
            {
                var contentPresenter = this.Content as ContentPresenter;
                return contentPresenter != null && contentPresenter.Content != null;
            }
        }

        public string BlockCaption
        {
            get { return (string)GetValue(BlockCaptionProperty); }
            set { SetValue(BlockCaptionProperty, value); }
        }

        public bool IsShaded
        {
            get { return (bool)GetValue(IsShadedProperty); }
            set { SetValue(IsShadedProperty, value); }
        }

        public string ApplyCaption
        {
            get { return (string)GetValue(ApplyCaptionProperty); }
            set { SetValue(ApplyCaptionProperty, value); }
        }

        public string ClearCaption
        {
            get { return (string)GetValue(ClearCaptionProperty); }
            set { SetValue(ClearCaptionProperty, value); }
        }

        public FrameworkElement ModalScopeElement
        {
            get { return (FrameworkElement)GetValue(ModalScopeElementProperty); }
            set { SetValue(ModalScopeElementProperty, value); }
        }

        public event EventHandler Applied;
        public void OnApplied(EventArgs e)
        {
            if (Applied != null)
            {
                Applied(this, e);
            }
        }

        public static RoutedCommand ApplyAction { get; private set; }
        protected virtual void Apply(object sender, ExecutedRoutedEventArgs e)
        {
            OnApplied(EventArgs.Empty);
        }

        public static RoutedCommand ClearAction { get; private set; }
        protected virtual void Clear(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close(null, null);
        }

        public event EventHandler Opened;
        public void OnOpened(EventArgs e)
        {
            if (Opened != null)
            {
                Opened(this, e);
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

        public static RoutedCommand CloseAction { get; private set; }

        public virtual void Open(object sender, ExecutedRoutedEventArgs e)
        {
            OnOpened(EventArgs.Empty);
            this.Visibility = Visibility.Visible;
            this.SetValue(IsOpenedProperty, true);
            //ModalSpace.ShowModal(this, true);
        }

        public virtual void Close(object sender, ExecutedRoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            this.SetValue(IsOpenedProperty, false);
            OnClosed(EventArgs.Empty);
            //ModalSpace.ShowModal(this, false);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            //System.Diagnostics.Debug.WriteLine(string.Format("Form PropertyChanged: {0}={1}", e.Property.Name, e.NewValue));
            if (e.Property.Name == "IsVisible")
            {
                ModalSpace.ShowModal(this, (bool)e.NewValue);
            }
        }
    }
}
