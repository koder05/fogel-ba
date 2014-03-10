using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RF.WinApp
{
    [TemplatePart(Name = SplitElementName, Type = typeof(UIElement))]
    public class SplitButton : MenuItem
    {
        static SplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));
            ApplyAction = new RoutedCommand("ApplyAction", typeof(SplitButton));
        }

        public static RoutedCommand ApplyAction { get; private set; }
        public virtual void Apply(object sender, ExecutedRoutedEventArgs e)
        {
            ClickOn();
        }

        /// <summary>
        /// Stores the public name of the split element.
        /// </summary>
        private const string SplitElementName = "SplitElement";

        /// <summary>
        /// Stores a reference to the split element.
        /// </summary>
        private UIElement _splitElement;

        /// <summary>
        /// Stores a reference to the ContextMenu.
        /// </summary>
        private ContextMenu _contextMenu;

        /// <summary>
        /// Stores a reference to the ancestor of the ContextMenu added as a logical child.
        /// </summary>
        private DependencyObject _logicalChild;

        /// <summary>
        /// Stores the initial location of the ContextMenu.
        /// </summary>
        private Point _contextMenuInitialOffset;

        /// <summary>
        /// Stores the backing collection for the ButtonMenuItemsSource property.
        /// </summary>
        private ObservableCollection<object> _buttonMenuItemsSource = new ObservableCollection<object>();

        /// <summary>
        /// Gets the collection of items for the split button's menu.
        /// </summary>
        public Collection<object> ButtonMenuItemsSource { get { return _buttonMenuItemsSource; } }

        /// <summary>
        /// Gets or sets a value indicating whetherthe mouse is over the split element.
        /// </summary>
        public bool IsMouseOverSplitElement
        {
            get
            {
                var splitelement = (FrameworkElement)Template.FindName(SplitElementName, this);
                return IsMouseWithin(splitelement, Mouse.PrimaryDevice.GetPosition(splitelement));
            }
        }

        public SplitButton()
        {
            //var style = Application.Current.TryFindResource(typeof(SplitButton)) as Style;
            //this.SetValue(FrameworkElement.StyleProperty, style);
        }

        public override void OnApplyTemplate()
        {
            // Unhook existing handlers
            if (null != _splitElement)
            {
                _splitElement = null;
            }
            if (null != _contextMenu)
            {
                _contextMenu.Opened -= new RoutedEventHandler(ContextMenu_Opened);
                _contextMenu.Closed -= new RoutedEventHandler(ContextMenu_Closed);
                _contextMenu = null;
            }
            if (null != _logicalChild)
            {
                RemoveLogicalChild(_logicalChild);
                _logicalChild = null;
            }

            // Apply new template
            base.OnApplyTemplate();

            // Hook new event handlers
            _splitElement = GetTemplateChild(SplitElementName) as UIElement;
            if (null != _splitElement)
            {
                _contextMenu = ContextMenuService.GetContextMenu(_splitElement);
                if (null != _contextMenu)
                {
                    // Add the ContextMenu as a logical child (for DataContext and RoutedCommands)
                    _contextMenu.IsOpen = true;
                    DependencyObject current = _contextMenu;
                    do
                    {
                        _logicalChild = current;
                        current = LogicalTreeHelper.GetParent(current);
                    } while (null != current);
                    _contextMenu.IsOpen = false;
                    AddLogicalChild(_logicalChild);

                    _contextMenu.Opened += new RoutedEventHandler(ContextMenu_Opened);
                    _contextMenu.Closed += new RoutedEventHandler(ContextMenu_Closed);
                }
            }

            var button = (Button)Template.FindName("Chrome", this);
            button.PreviewMouseLeftButtonDown += button_MouseLeftButtonDown;
            CommandBindings.Add(new CommandBinding(ApplyAction, Apply));
        }

        private void button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.OnMouseLeftButtonDown(e);
            e.Handled = false;
        }

        /// <summary>
        /// Called when the Button is clicked.
        /// </summary>
        protected void ClickOn()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("ClickOn, IsMouseOverSplitElement={0}", IsMouseOverSplitElement));
            if (nockick == false)
            {
                if (IsMouseOverSplitElement)
                {
                    System.Diagnostics.Debug.WriteLine("OpenButtonMenu");
                    OpenButtonMenu();
                }
                else
                {
                    this.OnClick();
                }
            }
            else
            {
                nockick = false;
            }
        }

        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (null == e)
            {
                throw new ArgumentNullException("e");
            }

            if ((Key.Down == e.Key) || (Key.Up == e.Key))
            {
                // WPF requires this to happen via BeginInvoke
                Dispatcher.BeginInvoke((Action)(() => OpenButtonMenu()));
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        /// <summary>
        /// Opens the button menu.
        /// </summary>
        protected void OpenButtonMenu()
        {
            if ((0 < _buttonMenuItemsSource.Count) && (null != _contextMenu))
            {
                _contextMenu.HorizontalOffset = 0;
                _contextMenu.VerticalOffset = 0;
                _contextMenu.IsOpen = true;
            }
        }

        /// <summary>
        /// Called when the ContextMenu is opened.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            // Offset the ContextMenu correctly
            _contextMenuInitialOffset = TranslatePoint(new Point(ActualWidth - 25, ActualHeight), _contextMenu);
            UpdateContextMenuOffsets();

            // Hook LayoutUpdated to handle application resize and zoom changes
            LayoutUpdated += new EventHandler(SplitButton_LayoutUpdated);
        }

        /// <summary>
        /// Called when the ContextMenu is closed.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            // No longer need to handle LayoutUpdated
            LayoutUpdated -= new EventHandler(SplitButton_LayoutUpdated);
            nockick = IsMouseOverSplitElement;
            System.Diagnostics.Debug.WriteLine(string.Format("ContextMenu_Closed, IsMouseOverSplitElement={0}", IsMouseOverSplitElement));
        }

        private bool nockick = false;

        /// <summary>
        /// Called when the ContextMenu is open and layout is updated.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        private void SplitButton_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateContextMenuOffsets();
        }

        /// <summary>
        /// Updates the ContextMenu's Horizontal/VerticalOffset properties to keep it under the SplitButton.
        /// </summary>
        private void UpdateContextMenuOffsets()
        {
            // Calculate desired offset to put the ContextMenu below and left-aligned to the Button
            Point currentOffset = new Point(0, -1);
            Point desiredOffset = _contextMenuInitialOffset;
            _contextMenu.HorizontalOffset = desiredOffset.X - currentOffset.X;
            _contextMenu.VerticalOffset = desiredOffset.Y - currentOffset.Y;
            // Adjust for RTL
            if (FlowDirection.RightToLeft == FlowDirection)
            {
                _contextMenu.HorizontalOffset *= -1;
            }
        }

        private bool IsMouseWithin(FrameworkElement element, Point point)
        {
            return element.ActualWidth >= point.X && point.X >= 0 && element.ActualHeight >= point.Y && point.Y >= 0;
        }
    }
}
