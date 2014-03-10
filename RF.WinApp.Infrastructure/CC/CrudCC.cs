using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections.ObjectModel;

using RF.WinApp.JIT;
using RF.Common;

namespace RF.WinApp
{
    [TemplatePart(Name = RowsTextboxTemplatePartName, Type = typeof(TextBox))]
    [TemplatePart(Name = FilterTemplatePartName, Type = typeof(FilterBlockCC))]
    [TemplatePart(Name = FormEditTemplatePartName, Type = typeof(FormCC))]
    [TemplatePart(Name = FormNewTemplatePartName, Type = typeof(FormCC))]
    public class CrudCC : DataGrid, INotifyPropertyChanged
    {
        public static readonly DependencyProperty FilterProperty;
        public static readonly DependencyProperty FormEditProperty;
        public static readonly DependencyProperty FormNewProperty;
        public static readonly DependencyProperty FilterBlockWidthProperty;
        public static readonly DependencyProperty FilterBlockHeightProperty;
        public static readonly DependencyProperty FormEditBlockWidthProperty;
        public static readonly DependencyProperty FormEditBlockHeightProperty;
        public static readonly DependencyProperty FormNewBlockWidthProperty;
        public static readonly DependencyProperty FormNewBlockHeightProperty;
        public static readonly DependencyProperty DataViewProviderProperty;
        public static readonly DependencyProperty IsBindableProperty;

        public const string FormNewTemplatePartName = "PART_FormNewCC";
        public const string FormEditTemplatePartName = "PART_FormEditCC";
        public const string FilterTemplatePartName = "PART_FilterBlockCC";
        public const string RowsTextboxTemplatePartName = "PART_RowsTB";

        static CrudCC()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CrudCC), new FrameworkPropertyMetadata(typeof(CrudCC)));

            FilterProperty = DependencyProperty.Register("Filter", typeof(ContentControl), typeof(CrudCC), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
            FormEditProperty = DependencyProperty.Register("FormEdit", typeof(ContentControl), typeof(CrudCC), new UIPropertyMetadata(null));
            FormNewProperty = DependencyProperty.Register("FormNew", typeof(ContentControl), typeof(CrudCC), new UIPropertyMetadata(null));
            DataViewProviderProperty = DependencyProperty.Register("DataViewProvider", typeof(Type), typeof(CrudCC), new UIPropertyMetadata(typeof(object), OnChangeDataViewProvider));

            IsBindableProperty = DependencyProperty.Register("IsBindable", typeof(bool), typeof(CrudCC), new UIPropertyMetadata(true, null, OnCoerceIsBindable));

            FilterBlockWidthProperty = DependencyProperty.Register("FilterBlockWidth", typeof(double), typeof(CrudCC), new UIPropertyMetadata(0.0));
            FilterBlockHeightProperty = DependencyProperty.Register("FilterBlockHeight", typeof(double), typeof(CrudCC), new UIPropertyMetadata(0.0));
            FormEditBlockWidthProperty = DependencyProperty.Register("FormEditBlockWidth", typeof(double), typeof(CrudCC), new UIPropertyMetadata(0.0, OnFormEditBlockWidthChanged));
            FormEditBlockHeightProperty = DependencyProperty.Register("FormEditBlockHeight", typeof(double), typeof(CrudCC), new UIPropertyMetadata(0.0, OnFormEditBlockHeightChanged));
            FormNewBlockWidthProperty = DependencyProperty.Register("FormNewBlockWidth", typeof(double), typeof(CrudCC), new UIPropertyMetadata(0.0, null, OnCoerceFormNewBlockWidth));
            FormNewBlockHeightProperty = DependencyProperty.Register("FormNewBlockHeight", typeof(double), typeof(CrudCC), new UIPropertyMetadata(0.0, null, OnCoerceFormNewBlockHeight));

            MoveFirstAction = new RoutedCommand("MoveFirstAction", typeof(CrudCC));
            MoveBackwardAction = new RoutedCommand("MoveBackwardAction", typeof(CrudCC));
            MoveForwardAction = new RoutedCommand("MoveForwardAction", typeof(CrudCC));
            MoveLastAction = new RoutedCommand("MoveLastAction", typeof(CrudCC));
            ToggleFilterAction = new RoutedCommand("ToggleFilterAction", typeof(CrudCC));
            ToggleFormAction = new RoutedCommand("ToggleFormAction", typeof(CrudCC));
            NewRowAction = new RoutedCommand("NewRowAction", typeof(CrudCC));
            DeleteAction = new RoutedCommand("DeleteAction", typeof(CrudCC));
        }

        private static void OnFormEditBlockWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(FormNewBlockWidthProperty);
        }

        private static object OnCoerceIsBindable(DependencyObject d, object baseValue)
        {
            var vm = (CrudCC)d;
            if ((bool)baseValue)
            {
                if (vm.ItemsSource == null)
                {
                    vm.Bind(() => vm.ScrollToModel(vm.SelectedModel));
                }
                else
                {
                    vm.ScrollToModel(vm.SelectedModel);
                }
            }
            return baseValue;
        }

        private static object OnCoerceFormNewBlockWidth(DependencyObject d, object baseValue)
        {
            var vm = (CrudCC)d;
            return vm.FormNewBlockWidth == 0.0 ? vm.FormEditBlockWidth : (double)baseValue;
        }

        private static void OnFormEditBlockHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(FormNewBlockHeightProperty);
        }

        private static object OnCoerceFormNewBlockHeight(DependencyObject d, object baseValue)
        {
            var vm = (CrudCC)d;
            return vm.FormNewBlockHeight == 0.0 ? vm.FormEditBlockHeight : (double)baseValue;
        }

        private static void OnChangeDataViewProvider(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = (CrudCC)d;
            vm.DataViewProvider = new DataViewProviderFactory().GetInstance((Type)e.NewValue);
        }

        private PageManager cache;
        private ObservableCollection<DataObj> fakeSource;
        private List<int> updatedWindow = new List<int>();
        private bool updatedWindowLock = false;
        private bool needUpdated = true;
        private int previousSelectedIndex = 0;

        public CrudCC()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;

            this.AddHandler(ScrollBar.ScrollEvent, new ScrollEventHandler(ScrollHandler));
            this.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(ScrollChangedHandler));
            this.AddHandler(VirtualizingStackPanel.CleanUpVirtualizedItemEvent, new CleanUpVirtualizedItemEventHandler(CleanUpVirtualizedItemHandler));

            this.SelectedCellsChanged += SelectedCellsChangedHandler;
            this.LoadingRow += LoadingRowHandler;
            this.Sorting += SortingHandler;

            CommandBindings.Add(new CommandBinding(MoveFirstAction, MoveFirst));
            CommandBindings.Add(new CommandBinding(MoveBackwardAction, MoveBackward));
            CommandBindings.Add(new CommandBinding(MoveForwardAction, MoveForward));
            CommandBindings.Add(new CommandBinding(MoveLastAction, MoveLast));
            CommandBindings.Add(new CommandBinding(ToggleFilterAction, ToggleFilter));
            CommandBindings.Add(new CommandBinding(ToggleFormAction, ToggleForm));
            CommandBindings.Add(new CommandBinding(NewRowAction, NewRow));
            CommandBindings.Add(new CommandBinding(DeleteAction, Delete));
            //VirtualizingStackPanel.SetIsVirtualizing(this, true);
        }

        /// <summary>
        /// Asynchronous bind grid to data
        /// </summary>
        private void Bind(Action syncCallbackTask)
        {
            AsyncHelper.Stitch(() =>
                {
                    cache.Reset();
                    List<DataObj> data = new List<DataObj>(cache.RowCount);
                    if (cache.RowCount <= cache.PageSize)
                    {
                        //virtualization off
                        for (int i = 0; i < cache.RowCount; i++)
                            data.Add(cache.RetrieveElement(i));
                    }
                    else
                    {
                        for (int i = 0; i < cache.RowCount; i++)
                            data.Add(new DataObj());
                    }
                    fakeSource = new ObservableCollection<DataObj>(data);
                }
                , () =>
                {
                    this.ItemsSource = fakeSource;
                    if (syncCallbackTask != null)
                        syncCallbackTask.Invoke();
                });
        }

        /// <summary>
        /// Asynchronous scroll grid to index
        /// </summary>
        private bool ScrollToIndex(int index, Action syncCallbackTask)
        {
            if (index < 0 || index >= this.Items.Count)
                return false;

            previousSelectedIndex = this.SelectedIndex;
            var o = fakeSource[index];

            AsyncHelper.Stitch(() =>
            {
                if (o.StaticStateVersion != cache.StaticStateVersion)
                    o = cache.RetrieveElement(index);
            },
            () =>
            {
                fakeSource[index] = o;
                this.ScrollIntoView(o);
                this.SelectedItem = o;
                
                if (syncCallbackTask != null)
                    syncCallbackTask.Invoke();
                
                Dispatcher.BeginInvoke(new Action(() => ScrollChangedHandler(null, null)), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            });

            return true;
        }

        private bool ScrollToIndex(int index)
        {
            return ScrollToIndex(index, null);
        }

        public void ScrollToModel(object model)
        {
            if (cache == null || this.DataViewProvider == null)
                return;

            if (this.ItemsSource == null)
                return;

            if (model == null)
            {
                //ScrollToIndex(0, () => { this.SelectedItem = null; this.SelectedModel = null; });
                this.SelectedItem = null; this.SelectedModel = null;
            }
            else
            {
                AsyncHelper.Stitch<int>((o) => cache.GetElementIndex(o),
                    (o, newIndex) =>
                    {
                        //this.Items.Refresh();
                        if (newIndex < 0)
                            this.SelectedItem = null;
                        ScrollToIndex(newIndex, () => this.Items.Refresh());
                    }, model);
            }
        }

        private void LoadingRowHandler(object sender, DataGridRowEventArgs e)
        {
            if (updatedWindowLock == false)
            {
                updatedWindow.Add(e.Row.GetIndex());
                var o = e.Row.Item as DataObj;
            }
        }

        private void CleanUpVirtualizedItemHandler(object sender, CleanUpVirtualizedItemEventArgs e)
        {
            if (updatedWindowLock == false)
            {
                int i = (e.UIElement as DataGridRow).GetIndex();
                updatedWindow.Remove(i);
            }
        }

        private void ScrollChangedHandler(object sender, ScrollChangedEventArgs e)
        {
            if (needUpdated && cache != null && updatedWindow.Count > 0)
            {
                updatedWindowLock = true;

                foreach (int index in updatedWindow)
                {
                    if (fakeSource[index].StaticStateVersion != cache.StaticStateVersion)
                    {
                        AsyncHelper.Stitch<DataObj>((i) =>
                        {
                            return cache.RetrieveElement((int)i);
                        },
                       (i, o) =>
                       {
                           //fakeSource[(int)i] = o;
                           fakeSource[(int)i].Model = o.Model;
                           fakeSource[(int)i].StaticStateVersion = o.StaticStateVersion;
                           System.Diagnostics.Debug.WriteLine(string.Format("ScrollChangedHandler index={0}", i));
                       }, index);
                    }
                }

                updatedWindow.Clear();
                updatedWindowLock = false;
            }
        }

        public void Refresh()
        {
            if (cache != null)
            {
                if (this.IsBindable)
                {
                    Bind(() => ScrollToIndex(0));
                }
            }
        }

        public event EventHandler TemplateApplied;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //ScrollViewer scrollViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            //scrollViewer.ScrollChanged += ScrollChanged;
            //ScrollBar scrollBar = GetTemplateChild("PART_VerticalScrollBar") as ScrollBar;
            //scrollBar.Scroll += Scroll;

            var filterBlock = Template.FindName(CrudCC.FilterTemplatePartName, this) as FilterBlockCC;
            if (filterBlock != null)
            {
                filterBlock.FilteredModelType = this.DataViewProvider.ModelType;
                filterBlock.FilterApplied += FilterHandler;
            }

            var updFormBlock = Template.FindName(CrudCC.FormEditTemplatePartName, this) as FormCC;
            if (updFormBlock != null)
            {
                //updFormBlock.AddHandler(Binding.SourceUpdatedEvent, new EventHandler<DataTransferEventArgs>(SourceUpdatedHandler));
                updFormBlock.Applied += FormUpdate;
                updFormBlock.DataContextChangeCanceled += FormUpdateContextChangeCanceledHandler;
            }

            var newFormBlock = Template.FindName(CrudCC.FormNewTemplatePartName, this) as FormCC;
            if (newFormBlock != null)
            {
                newFormBlock.Opened += NewFormOpen;
                newFormBlock.Applied += NewFormSave;
            }

            if (this.DataViewProvider != null && cache == null)
            {
                cache = new PageManager(300, 1, 0, this.DataViewProvider);
            }

            if (TemplateApplied != null)
                TemplateApplied(this, EventArgs.Empty);
        }

        public ContentControl Filter
        {
            get { return (ContentControl)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public ContentControl FormEdit
        {
            get { return (ContentControl)GetValue(FormEditProperty); }
            set { SetValue(FormEditProperty, value); }
        }

        public ContentControl FormNew
        {
            get { return (ContentControl)GetValue(FormNewProperty); }
            set { SetValue(FormNewProperty, value); }
        }

        public IDataView DataViewProvider { get; set; }

        public bool IsBindable
        {
            get { return (bool)GetValue(IsBindableProperty); }
            set { SetValue(IsBindableProperty, value); }
        }

        private object _selectedModel;
        public object SelectedModel
        {
            get
            {
                return _selectedModel;
            }
            set
            {
                if ((_selectedModel != null && !_selectedModel.Equals(value)) || (_selectedModel == null && value != null))
                {
                    _selectedModel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedModel"));
                }
            }
        }

        public double FilterBlockWidth
        {
            get { return (double)GetValue(FilterBlockWidthProperty); }
            set { SetValue(FilterBlockWidthProperty, value); }
        }

        public double FilterBlockHeight
        {
            get { return (double)GetValue(FilterBlockHeightProperty); }
            set { SetValue(FilterBlockHeightProperty, value); }
        }

        public double FormEditBlockWidth
        {
            get { return (double)GetValue(FormEditBlockWidthProperty); }
            set { SetValue(FormEditBlockWidthProperty, value); }
        }

        public double FormEditBlockHeight
        {
            get { return (double)GetValue(FormEditBlockHeightProperty); }
            set { SetValue(FormEditBlockHeightProperty, value); }
        }

        public double FormNewBlockWidth
        {
            get { return (double)GetValue(FormNewBlockWidthProperty); }
            set { SetValue(FormNewBlockWidthProperty, value); }
        }

        public double FormNewBlockHeight
        {
            get { return (double)GetValue(FormNewBlockHeightProperty); }
            set { SetValue(FormNewBlockHeightProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private void SelectedCellsChangedHandler(object sender, SelectedCellsChangedEventArgs e)
        {
            var tbRows = Template.FindName(CrudCC.RowsTextboxTemplatePartName, this) as TextBox;
            if (tbRows != null)
                tbRows.Text = string.Format("{0} из {1}", this.SelectedIndex + 1, this.Items.Count);

            var dobj = this.SelectedItem as DataObj;
            if (dobj != null)
                this.SelectedModel = dobj.Model;
        }

        private void SortingHandler(object sender, DataGridSortingEventArgs e)
        {
            if (this.SelectedItem != null && cache != null && cache.RowCount > cache.PageSize)
            {
                DataGridColumn col = e.Column;
                var sortDirection = (col.SortDirection ?? ListSortDirection.Descending) == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                foreach (DataGridColumn c in this.Columns)
                {
                    c.SortDirection = null;
                    cache.RemoveSortOrder(c.SortMemberPath.Replace("Model.", ""));
                }

                cache.AddSortOrder(col.SortMemberPath.Replace("Model.", ""), sortDirection);
                col.SortDirection = sortDirection;
                cache.ResetPages();
                e.Handled = true;
                ScrollToModel((this.SelectedItem as DataObj).Model);
            }
        }

        private void ScrollHandler(object sender, ScrollEventArgs e)
        {
            if (e.ScrollEventType == ScrollEventType.ThumbTrack)
            {
                needUpdated = false;
            }
            else if (e.ScrollEventType == ScrollEventType.EndScroll)
            {
                needUpdated = true;
                ScrollChangedHandler(sender, null);
            }
        }

        private void FilterHandler(object sender, FilterBlockEventArgs e)
        {
            if (cache != null)
            {
                cache.Filters = e.Filters;
                this.Refresh();
            }
        }

        private void FormUpdateContextChangeCanceledHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            ScrollToIndex(previousSelectedIndex);
        }

        public static RoutedCommand MoveFirstAction { get; private set; }
        protected void MoveFirst(object sender, ExecutedRoutedEventArgs e)
        {
            MoveFirst();
        }

        public static RoutedCommand MoveBackwardAction { get; private set; }
        protected void MoveBackward(object sender, ExecutedRoutedEventArgs e)
        {
            MoveBackward();
        }

        public static RoutedCommand MoveForwardAction { get; private set; }
        protected void MoveForward(object sender, ExecutedRoutedEventArgs e)
        {
            MoveForward();
        }

        public static RoutedCommand MoveLastAction { get; private set; }
        protected void MoveLast(object sender, ExecutedRoutedEventArgs e)
        {
            MoveLast();
        }

        public virtual bool MoveFirst()
        {
            return ScrollToIndex(0);
        }

        public virtual bool MoveBackward()
        {
            return ScrollToIndex(GotoRow() ?? this.SelectedIndex - 1);
        }

        public virtual bool MoveForward()
        {
            return ScrollToIndex(GotoRow() ?? this.SelectedIndex + 1);
        }

        public virtual bool MoveLast()
        {
            return ScrollToIndex(this.Items.Count - 1);
        }

        private int? GotoRow()
        {
            int? ret = null;
            var tbRows = Template.FindName(CrudCC.RowsTextboxTemplatePartName, this) as TextBox;
            if (tbRows != null)
            {
                int i = -1;
                int.TryParse(tbRows.Text, out i);
                if (i > 0 && i <= this.Items.Count)
                    ret = i - 1;
            }
            return ret;
        }

        public static RoutedCommand ToggleFilterAction { get; private set; }
        protected virtual void ToggleFilter(object sender, ExecutedRoutedEventArgs e)
        {
            var form = Template.FindName(CrudCC.FilterTemplatePartName, this) as ActionBlock;
            ToggleBlock(form);
        }

        public static RoutedCommand ToggleFormAction { get; private set; }
        protected virtual void ToggleForm(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.SelectedModel == null)
            {
                NewRow(sender, e);
            }
            else
            {
                var form = Template.FindName(CrudCC.FormEditTemplatePartName, this) as ActionBlock;
                ToggleBlock(form);
            }
        }

        public static RoutedCommand NewRowAction { get; private set; }
        protected virtual void NewRow(object sender, ExecutedRoutedEventArgs e)
        {
            ToggleBlock(Template.FindName(CrudCC.FormNewTemplatePartName, this) as ActionBlock);
        }

        public static RoutedCommand DeleteAction { get; private set; }
        protected virtual void Delete(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format("Предполагается удаление {0} записей.{1}Продолжить?", this.SelectedItems.Count, Environment.NewLine), "Вопрос"
                        , MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes && cache != null)
            {
                var deleteList = GetSelectedItems().ToList();
                AsyncHelper.Stitch(() => this.DataViewProvider.Delete(deleteList), () => this.Refresh());
            }
        }

        private IEnumerable<object> GetSelectedItems()
        {
            foreach (var o in this.SelectedItems)
                yield return (o as DataObj).Model;
        }

        private void NewFormOpen(object sender, EventArgs e)
        {
            var o = new DataObj();
            o.Model = this.DataViewProvider.ActivateEmptyModel();
            var newFormBlock = sender as FormCC;
            newFormBlock.DataContext = o;
        }

        private void NewFormSave(object sender, EventArgs e)
        {
            var newFormBlock = sender as FormCC;
            if (newFormBlock != null && cache != null)
            {
                var o = newFormBlock.DataContext as DataObj;
                if (o != null && o.IsEditing)
                {
                    int newIndex = -1;
                    AsyncHelper.Stitch(() =>
                        {
                            this.DataViewProvider.Create(o.Model);
                            o.IsEditing = false;
                            newIndex = cache.GetElementIndex(o.Model);

                        }
                    , () => Bind(() => ScrollToIndex(newIndex < 0 ? 0 : newIndex, () => newFormBlock.Close(null, null)))
                    );
                }
                else
                    newFormBlock.Close(null, null);
            }
        }

        private void FormUpdate(object sender, EventArgs e)
        {
            var updateFormBlock = sender as FormCC;
            if (updateFormBlock != null && cache != null)
            {
                var o = updateFormBlock.DataContext as DataObj;
                if (o != null && o.IsEditing)
                {
                    AsyncHelper.Stitch(() =>
                    {
                        this.DataViewProvider.Update(o.Model);
                        o.IsEditing = false;
                        cache.ResetPages();
                    }
                    , () =>
                    {
                        this.ScrollToModel(o.Model); 
                        //ScrollToIndex(0);
                        updateFormBlock.Close(null, null);
                    });
                }
                else
                    updateFormBlock.Close(null, null);
            }
        }

        private void ToggleBlock(ActionBlock block)
        {
            if (block != null)
            {
                if (block.IsOpened)
                    block.Close(block, null);
                else if (block.IsNotEmpty)
                    block.Open(block, null);
            }
        }
    }
}
