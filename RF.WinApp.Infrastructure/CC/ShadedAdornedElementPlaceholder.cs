using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RF.WinApp
{
    public class ShadedAdornedElementPlaceholder : AdornedElementPlaceholder
    {
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (this.AdornedElement != null)
            {
                Binding b1 = new Binding();
                b1.Path = new PropertyPath(ActionBlock.IsShadedProperty);
                b1.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ActionBlock), 1);
                BindingOperations.SetBinding(this.AdornedElement, ActionBlock.IsShadedProperty, b1);

                Binding b2 = new Binding();
                b2.Path = new PropertyPath(ActionBlock.IsShadedProperty);
                b2.Source = this.AdornedElement;
                BindingOperations.SetBinding(this, ActionBlock.IsShadedProperty, b2);
            }
        }
    }
}
