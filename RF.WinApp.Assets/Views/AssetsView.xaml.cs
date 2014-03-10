using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;

using RF.WinApp.UC;
using RF.BL.Model.Enums;
using RF.Common;
using RF.BL.Model;
using RF.WinApp.Infrastructure.Behaviour;

namespace RF.WinApp.Assets.Views
{
    [Export("AssetsView")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class AssetsView : UserControl
    {
        public static RoutedCommand FileBrowseAction { get; private set; }

        static AssetsView()
        {
            FileBrowseAction = new RoutedCommand("FileBrowseAction", typeof(AssetsView));
        }

        public IEnumerable<EnumViewModel<InsuranceType>> InsuranceTypes
        {
            get
            {
                return new EnumViewModel<InsuranceType>().GetList();
            }
        }

        private class ImportFormViewModel
        {
            [Required]
            public EnumViewModel<InsuranceType> InsuranceType { get; set; }

            [Required]
            public string ExcelFile { get; set; }

            [Required]
            public string ExcelDataSheet { get; set; }
        }

        private class ReportFormViewModel
        {
            public ReportFormViewModel()
            {
                DateTime tillDate = DateTime.Today;
                DateTime fromDate = new DateTime(tillDate.Year, tillDate.Month, 1);
                this.DateBegin = fromDate;
                this.DateEnd = fromDate.AddMonths(1).AddDays(-1);
            }

            [Required]
            public EnumViewModel<InsuranceType> InsuranceType { get; set; }

            [Required]
            public DateTime? DateBegin { get; set; }

            [Required]
            public DateTime? DateEnd { get; set; }

            public Governor Governor { get; set; }
        }

        public AssetsView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            CommandBindings.Add(new CommandBinding(FileBrowseAction, FileBrowse));
            xlsImport.Applied += Import;
            reportForm.Applied += Report;
        }

        private void Import(object sender, EventArgs e)
        {
            var form = sender as ActionBlock;
            if (!form.ValidateAll())
                return;

            var model = form.DataContext as ImportFormViewModel;
            var provider = AssetsCRUD.DataViewProvider as AssetsDataViewProvider;
            bool iscashflow = rbImportCashFlow.IsChecked.Value;
            AsyncHelper.Stitch(() => provider.ImportFromExcel(model.ExcelFile, model.ExcelDataSheet, model.InsuranceType.Value, iscashflow), () => { AssetsCRUD.Refresh(); xlsImport.Close(null, null); });
        }

        private void Report(object sender, EventArgs e)
        {
            var form = sender as ActionBlock;
            if (!form.ValidateAll())
                return;

            var model = form.DataContext as ReportFormViewModel;
            var provider = AssetsCRUD.DataViewProvider as AssetsDataViewProvider;
            AsyncHelper.Stitch(() => provider.PublicAssetProfitReport(model.DateBegin.Value, model.DateEnd.Value, model.InsuranceType.Value, model.Governor), () => reportForm.Close(null, null));
        }

        private void ImportExcelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            xlsImport.Open(null, null);
            xlsImport.DataContext = new ImportFormViewModel();
        }

        private void FileBrowse(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xls;*.xlsx";
            dlg.Filter = "Файлы Excel |*.xls;*.xlsx";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                tbImportFileName.Text = filename;
                tbImportFileName.ToolTip = filename;
                var sheets = (AssetsCRUD.DataViewProvider as AssetsDataViewProvider).ExcelDataSheets(filename);
                cbDataSheet.ItemsSource = sheets;
                if (sheets != null && sheets.Count() == 1)
                    cbDataSheet.SelectedItem = sheets.ElementAt(0);
            }
        }

        private void ReportButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            reportForm.Open(null, null);
            reportForm.DataContext = new ReportFormViewModel();
        }
    }
}
