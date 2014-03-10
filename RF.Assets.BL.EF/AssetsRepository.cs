using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

using RF.BL.Model;
using RF.BL.Model.Enums;
using RF.BL.EF.Repositories;
using RF.Assets.BL;
using RF.LinqExt;
using EF;
using EF.Sql;
using RF.BL.Excel;
using RF.Excel;
using RF.Common.Transactions;
using RF.Reporting;

namespace RF.BL.EF
{
    public class AssetsRepository : CUDRepository<AssetValue>, IAssetsRepository
    {
        private IFilterOperatorResolver opResolver = new SqlOperatorResolver();
        private IFilterSortPropResolver propResolver = new AssetsPropResolver();
        private SortParameterCollection defaultSorting = new SortParameterCollection();
        private AssetsEFCtx _db;
        private IGovernorRepository _govRep;

        public AssetsRepository(AssetsEFCtx db, IGovernorRepository govRep)
            : base(db, null)
        {
            _db = db;
            _govRep = govRep;

            defaultSorting.Add<AssetValue>("TakingDate", System.ComponentModel.ListSortDirection.Descending);
            defaultSorting.Add<AssetValue>("GovernorId");
            defaultSorting.Add<AssetValue>("InsuranceTypeValue");
        }

        public int GetListCount(FilterParameterCollection filters)
        {
            lock (_db)
            {
                if (filters != null)
                    filters.PropertyNameResolver = propResolver;
                return _db.Assets.Filtering(filters, opResolver).Count();
            }
        }

        public IEnumerable<AssetValue> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy)
        {
            lock (_db)
            {
                orderBy.DefaultOrder = defaultSorting;
                orderBy.PropertyNameResolver = propResolver;
                if (filters != null)
                    filters.PropertyNameResolver = propResolver;
                var list = _db.Assets.Include("Governor").Include("Governor.Company").Filtering(filters, opResolver).Sorting(orderBy).Paging(pageIndex, pageSize);
                foreach (var m in list)
                    yield return m;
            }
        }

        public int GetIndexOf(AssetValue o, FilterParameterCollection filters, SortParameterCollection orderBy)
        {
            orderBy.DefaultOrder = defaultSorting;
            orderBy.PropertyNameResolver = propResolver;
            if (filters != null)
            {
                filters.OperatorActionResolver = opResolver;
                filters.PropertyNameResolver = propResolver;
            }
            return _db.Assets.GetIndexOf(filters, orderBy, poco => poco.Id == o.Id);
        }

        public AssetValue GetById(Guid id)
        {
            return _db.Assets.FirstOrDefault(poco => poco.Id == id);
        }

        public void ImportFromExcel(string excelFileName, string dataSheet, Model.Enums.InsuranceType insType, bool isCashFlow)
        {
            XlsObject<AssetValue> parser;
            if (isCashFlow)
                parser = new AssetsCashXlsObject(_govRep.GetList(null, 0, int.MaxValue, new SortParameterCollection()), insType, excelFileName, dataSheet);
            else
                parser = new AssetsValXlsObject(_govRep.GetList(null, 0, int.MaxValue, new SortParameterCollection()), insType, excelFileName, dataSheet);

            using (var trans = Transactions.StartNew())
            {
                foreach (var o in parser.SelectAll())
                {
                    this.Create(o);
                }
                trans.Complete();
            }
        }

        public IEnumerable<string> ExcelDataSheets(string excelFileName)
        {
            return ExcelReader.GetReader().GetSheetNames(excelFileName);
        }

        public string PublicAssetProfitReport(DateTime db, DateTime de, InsuranceType insType, Guid? govId)
        {
            ReportParameters pars = new ReportParameters("AssetProfitReport", ContentTypes.MicrosoftExcel, "AssetProfitReport.xls");
            //select * from [dbo].[add_fn_AssetCostReport] ('01.09.2012', '30.09.2012', 1) for xml raw('ReportRow'), root('ReportView')
            pars["Caption"] = "Report";

            var dbParam = new SqlParameter("@db", db.ToString("d"));
            //dbParam.SqlDbType = System.Data.SqlDbType.Date;

            var deParam = new SqlParameter("@de", de.ToString("d"));
            //deParam.SqlDbType = System.Data.SqlDbType.Date;

            var typeParam = new SqlParameter("@type", insType);

            var govIdParam = new SqlParameter("@govid", govId ?? (object)DBNull.Value);
            govIdParam.SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
            govIdParam.IsNullable = true;
            govIdParam.DbType = System.Data.DbType.Guid;

            XmlDocument doc = _db.SqlQueryXml("select * from [dbo].[add_fn_AssetCostReport] (convert(datetime, @db, 104), convert(datetime, @de, 104), @type, @govid)"
                , "ReportView", "ReportRow", dbParam, deParam, typeParam, govIdParam);
            //XmlDocument doc = _db.SqlQueryXml("select * from [dbo].[add_fn_AssetCostReport] (@db, @de, @type, @govid)", "ReportView", "ReportRow", dbParam, deParam, typeParam, govIdParam);
            //XmlDocument doc = _db.SqlQueryXml("select * from [dbo].[add_fn_AssetCostReport] ('01.04.2010', '31.03.2013', 1)", "ReportView", "ReportRow");
            doc.DocumentElement.SetAttribute("DateBegin", XmlConvert.ToString(db, "dd.MM.yyyy"));
            doc.DocumentElement.SetAttribute("DateEnd", XmlConvert.ToString(de, "dd.MM.yyyy"));
            doc.DocumentElement.SetAttribute("InsuranceType", insType.ToString());
            pars.AddXmlElement(doc.DocumentElement);

            //doc = _db.SqlQueryXml("select * from [dbo].[add_fn_AssetCostReportYearTotal] ('01.04.2010', '31.03.2013', 1)", "YearTotal", "YearTotalRow");
            //doc = _db.SqlQueryXml("select * from [dbo].[add_fn_AssetCostReportYearTotal] (@db, @de, @type, @govid)", "YearTotal", "YearTotalRow"
            doc = _db.SqlQueryXml("select * from [dbo].[add_fn_AssetCostReportYearTotal] (convert(datetime, @db, 104), convert(datetime, @de, 104), @type, @govid)", "YearTotal", "YearTotalRow"
                , (SqlParameter)(dbParam as ICloneable).Clone(), (SqlParameter)(deParam as ICloneable).Clone(), (SqlParameter)(typeParam as ICloneable).Clone(), (SqlParameter)(govIdParam as ICloneable).Clone());
            pars.AddXmlElement(doc.DocumentElement);

            //doc = _db.SqlQueryXml("select * from [dbo].[add_fn_AssetCostReportTotal] ('01.04.2010', '31.03.2013', 1)", "Total", "TotalRow");
            //doc = _db.SqlQueryXml("select * from [dbo].[add_fn_AssetCostReportTotal] (@db, @de, @type, @govid)", "Total", "TotalRow"
            doc = _db.SqlQueryXml("select * from [dbo].[add_fn_AssetCostReportTotal] (convert(datetime, @db, 104), convert(datetime, @de, 104), @type, @govid)", "Total", "TotalRow"
                , (SqlParameter)(dbParam as ICloneable).Clone(), (SqlParameter)(deParam as ICloneable).Clone(), (SqlParameter)(typeParam as ICloneable).Clone(), (SqlParameter)(govIdParam as ICloneable).Clone());
            pars.AddXmlElement(doc.DocumentElement);
            return ReportGenerator.GenerateReportToRawString(pars);
        }

        public IQueryable<AssetValue> Context
        {
            get
            {
                return _db.Assets;
            }
        }

        public override object Create(AssetValue o)
        {
            if (o.Governor == null)
            {
                o.Governor = _db.Governors.Find(o.GovernorId);
            }
            return base.Create(o);
        }

        public override void Update(AssetValue o)
        {
            if (o.Governor == null)
            {
                o.Governor = _db.Governors.Find(o.GovernorId);
            }
            base.Update(o);
        }
    }
}
