using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Media;

using Microsoft.Practices.Prism.Regions;

using RF.WinApp.ViewModel;
using RF.WinApp.Infrastructure.Models;
using RF.WinApp.Infrastructure.UIS;

namespace RF.WinApp
{
    [Export]
    public partial class ShellWindow : Window
    {
        public ShellWindow()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            regionManager.Regions[RegionNames.ModalRegion].Views.CollectionChanged += ModalRegionCollectionChanged;
        }

        public void WaitOverDoorOn()
        {
            this.MainFrame.SetWaitOverdoor(true);
        }

        public void WaitOverDoorOff()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.MainFrame.SetWaitOverdoor(false)), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        [Import]
        private IRegionManager regionManager;

        [Import]
        private RF.Common.Security.ILogonPage logonPage
        {
            set { RF.Common.Security.Logon.Page = value; }
        }

        [Import]
        private ShellWindowModel ViewModel
        {
            get { return this.DataContext as ShellWindowModel; }
            set { this.DataContext = value; }
        }

        private void ModalPlace_Applied(object sender, EventArgs e)
        {
            bool needClose = true;
            foreach (var view in regionManager.Regions[RegionNames.ModalRegion].Views.Cast<IModalView>())
            {
                needClose &= view.ModalApplied();
            }

            if (needClose)
                this.ModalPlace.Close(null, null);
        }

        private void ModalPlace_Closed(object sender, EventArgs e)
        {
            var removeList = new List<object>();
            foreach (var view in regionManager.Regions[RegionNames.ModalRegion].Views.Cast<IModalView>())
            {
                view.ModalClosed();
                removeList.Add(view);
            }

            //regionManager.Regions[RegionNames.ModalRegion].Views.CollectionChanged -= ModalRegionCollectionChanged;

            foreach (var view in removeList)
                regionManager.Regions[RegionNames.ModalRegion].Remove(view);

            //regionManager.Regions[RegionNames.ModalRegion].Views.CollectionChanged += ModalRegionCollectionChanged;
        }

        private void ModalRegionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                this.ModalPlace.Open(null, null);
                var view = e.NewItems[0] as IModalView;
                this.ModalPlace.BlockCaption = view.Caption;
                this.ModalPlace.Width = view.Width;
                this.ModalPlace.Height = view.Height;
                this.ModalPlace.ApplyCaption = view.ApplyCaption;
                this.ModalPlace.ClearCaption = view.ClearCaption;
            }
        }

        private void PlatesCC_RemovePlate(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (var view in e.OldItems)
                    if (regionManager.Regions[RegionNames.WorkspaceRegion].Views.Contains(view))
                        regionManager.Regions[RegionNames.WorkspaceRegion].Remove(view);
            }
        }

        private void MenuItem_Click(object sender, RadioMenuEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.CurrentKey) == false)
            {
                ThemeDictionaryExtension tde = new ThemeDictionaryExtension(e.CurrentKey);
                ResourceDictionary d = Application.Current.Resources.MergedDictionaries[1];
                var uri = tde.ProvideValue(new sp(d)) as Uri;
                if (d.Source.ToString() != uri.ToString())
                {
                    d.Source = uri;
                    //Application.Current.Resources.Remove(2);
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
                    if (regionManager != null && regionManager.Regions[RegionNames.MenuRegion] != null)
                        foreach (var view in regionManager.Regions[RegionNames.MenuRegion].Views.Cast<SplitButton>())
                        {
                            var style = view.TryFindResource(typeof(SplitButton)) as Style;
                            view.SetValue(FrameworkElement.StyleProperty, style);
                        }
                }
            }

            //d.Source = new Uri("pack://application:,,,/RF.WinApp.Themes.T1;component/Themes/generic.xaml");
        }
    }

    public class UISettingsAssistantShellwindow : UISettingsAssistantWindow
    {
        public override Type AttendedType
        {
            get
            {
                return typeof(ShellWindow);
            }
        }

        public UISettingsAssistantShellwindow(UISettingsStoreProvider storeProvider)
            : base(storeProvider)
        {
        }
    }

    internal class sp : IServiceProvider
    {
        private ResourceDictionary _d;
        public sp(ResourceDictionary d)
        {
            _d = d;
        }

        public object GetService(Type serviceType)
        {
            return new pvt(_d);
        }
    }

    internal class pvt : System.Windows.Markup.IProvideValueTarget
    {
        private ResourceDictionary _d;
        public pvt(ResourceDictionary d)
        {
            _d = d;
        }

        public object TargetObject
        {
            get { return _d; }
        }

        public object TargetProperty
        {
            get { return typeof(ResourceDictionary).GetProperty("Source"); }
        }
    }

}
