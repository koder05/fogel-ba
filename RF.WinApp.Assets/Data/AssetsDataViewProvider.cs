using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;

using RF.BL;
using RF.BL.Model;
using RF.BL.Model.Enums;
using RF.LinqExt;

namespace RF.WinApp
{
    public class AssetsDataViewProvider : IDataView
    {
        private IAssetsRepository _rep;

        public AssetsDataViewProvider(IAssetsRepository rep)
        {
            _rep = rep;
        }

        public Type ModelType { get { return typeof(AssetValue); } }

        public object ActivateEmptyModel()
        {
            AssetValue a = new AssetValue();
            a.TakingDate = DateTime.Today;
            a.InsuranceType = BL.Model.Enums.InsuranceType.OPS;
            //a.GovernorId = g.Id;
            a.Governor = null;
            return a;
        }

        public int GetListCount(FilterParameterCollection filters)
        {
            return _rep.GetListCount(filters);
        }

        public IEnumerable<object> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy)
        {
            return _rep.GetList(filters, pageIndex, pageSize, orderBy).Cast<object>().ToList();
        }

        public int GetIndexOf(object o, FilterParameterCollection filters, SortParameterCollection orderBy)
        {
            return _rep.GetIndexOf((AssetValue)o, filters, orderBy);
        }

        public void Create(object o)
        {
            var assets = o as AssetValue;
            assets.GovernorId = assets.Governor.Id;
            _rep.Create(assets);
        }

        public void Delete(IEnumerable<object> pool)
        {
            foreach (var o in pool)
                _rep.Delete(o as AssetValue);
        }

        public void Update(object o)
        {
            var assets = o as AssetValue;
            assets.GovernorId = assets.Governor.Id;
            _rep.Update(assets);
        }

        public void ImportFromExcel(string excelFileName, string dataSheet, InsuranceType insType, bool isCashFlow)
        {
            _rep.ImportFromExcel(excelFileName, dataSheet, insType, isCashFlow);
        }

        public IEnumerable<string> ExcelDataSheets(string excelFileName)
        {
            return _rep.ExcelDataSheets(excelFileName);
        }

        public void PublicAssetProfitReport(DateTime db, DateTime de, InsuranceType insType, Governor gov)
        {
            var rawReport = _rep.PublicAssetProfitReport(db, de, insType, gov != null ? gov.Id : (Guid?)null);

            string tempDir = Path.Combine(Path.GetTempPath(), "Access", "XsltReports");
            string filePath = Path.Combine(tempDir, string.Format("AssetProfitReport{0:yyMMddHHmm}.xml", DateTime.Now));
            Directory.CreateDirectory(tempDir);
            using (StreamWriter txtstream = File.CreateText(filePath))
            {
                try
                {
                    txtstream.Write(rawReport);
                }
                catch (Exception ex)
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    throw new InvalidOperationException(string.Format("Ошибка при генерации файла отчета. " + Environment.NewLine + "Ошибка: {0}", ex.Message), ex);
                }
            }

            //var p = new System.Diagnostics.Process();
            //p.StartInfo = new System.Diagnostics.ProcessStartInfo(filePath);
            //p.Start();

            System.Diagnostics.Process.Start(filePath);
        }
    }
}
