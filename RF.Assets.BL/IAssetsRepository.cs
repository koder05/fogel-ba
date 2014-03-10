using System;
using System.Collections.Generic;
using System.Linq;

using RF.BL.Model;
using RF.BL.Model.Enums;
using RF.LinqExt;

namespace RF.BL
{
    public interface IAssetsRepository : ICUDRepository<AssetValue>
    {
        IQueryable<AssetValue> Context { get; }
        AssetValue GetById(Guid id);
        int GetListCount(FilterParameterCollection filters);
        IEnumerable<AssetValue> GetList(FilterParameterCollection filters, int startRowIndex, int maximumRows, SortParameterCollection orderBy);
        int GetIndexOf(AssetValue o, FilterParameterCollection filters, SortParameterCollection orderBy);
        void ImportFromExcel(string excelFileName, string dataSheet, InsuranceType insType, bool isCashFlow);
        IEnumerable<string> ExcelDataSheets(string excelFileName);
        string PublicAssetProfitReport(DateTime db, DateTime de, InsuranceType insType, Guid? govId);
    }
}
