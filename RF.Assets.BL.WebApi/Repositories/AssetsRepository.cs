using System;
using System.Collections.Generic;
using System.Linq;

using RF.LinqExt;
using RF.BL;
using BLL = RF.BL.Model;
using RF.Assets.BL;
using WebApi.Svc;
using RF.BL.WebApi.DtoProxy;
using RF.BL.WebApi.Mapping;

using RF.BL.Model.Enums;
using RF.BL.Excel;
using RF.Excel;
using RF.Common.Transactions;
using RF.Reporting;

namespace RF.Assets.BL.WebApi
{
    public class AssetsRepository : IAssetsRepository
    {
        private IFilterSortPropResolver propResolver = new AssetsPropResolver();
        private SortParameterCollection defaultSorting = new SortParameterCollection();
        private WebApiCtx _db;
        private Model2DtoMapper _mapper;

        public AssetsRepository(WebApiCtx db, Model2DtoMapper mapper)
        {
            _db = db;
            _mapper = mapper;

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
                return _db.Assets.AddFilters(filters).TotalCount();
            }
        }

        public IEnumerable<BLL.AssetValue> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy)
        {
            lock (_db)
            {
                orderBy.DefaultOrder = defaultSorting;
                orderBy.PropertyNameResolver = propResolver;
                if (filters != null)
                    filters.PropertyNameResolver = propResolver;
                int skip = pageIndex * pageSize;
                var list = _db.Assets.AddFilters(filters).AddOrders(orderBy).Skip(skip).Take(pageSize);
                var govCtx = _db.Governors;
                foreach (var m in list)
                {
                    //provide action like '$expand' for Governors
                    var gov = govCtx.GetById(m.GovernorId);
                    if (gov == null)
                        gov = govCtx.ToList().FirstOrDefault(g => g.Id == m.GovernorId);
                    m.Governor = gov;

                    yield return ProxyActivator.CreateProxy<AssetValue, BLL.AssetValue>(m);
                }
            }
        }

        public int GetIndexOf(BLL.AssetValue o, FilterParameterCollection filters, SortParameterCollection orderBy)
        {
            orderBy.DefaultOrder = defaultSorting;
            orderBy.PropertyNameResolver = propResolver;
            if (filters != null)
                filters.PropertyNameResolver = propResolver;

            return _db.Assets.AddFilters(filters).AddOrders(orderBy).IndexOf(o.Id);
        }

        public BLL.AssetValue GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void ImportFromExcel(string excelFileName, string dataSheet, InsuranceType insType, bool isCashFlow)
        {
            var govs = _db.Entities
                .Where(e => e.Identity.Contains(string.Format("{0}(guid'", _db.Governors.RequestUri.PathAndQuery)))
                .Select(ed => ProxyActivator.CreateProxy<Governor, BLL.Governor>(ed.Entity as Governor)).ToList();
            XlsObject<BLL.AssetValue> parser;
            if (isCashFlow)
                parser = new AssetsCashXlsObject(govs, insType, excelFileName, dataSheet);
            else
                parser = new AssetsValXlsObject(govs, insType, excelFileName, dataSheet);

            var list = parser.SelectAll().Select(val => Newtonsoft.Json.JsonConvert.SerializeObject(val)).ToList();

            UriBuilder urib = new UriBuilder(_db.BaseUri);
            urib.Path = string.Format("{0}/CreateBatch", _db.Assets.RequestUri.PathAndQuery);
            var r = _db.Execute<bool>(urib.Uri, "POST", true
                , new System.Data.Services.Client.BodyOperationParameter("Values", list)
                ).FirstOrDefault();

            if (!r)
                throw new InvalidOperationException("ImportFromExcel");
        }

        public IEnumerable<string> ExcelDataSheets(string excelFileName)
        {
            return ExcelReader.GetReader().GetSheetNames(excelFileName);
        }

        public string PublicAssetProfitReport(DateTime db, DateTime de, InsuranceType insType, Guid? govId)
        {
            UriBuilder urib = new UriBuilder(_db.BaseUri);
            urib.Path = string.Format("{0}/Report", _db.Assets.RequestUri.PathAndQuery);
            return _db.Execute<string>(urib.Uri, "POST", true
                , new System.Data.Services.Client.BodyOperationParameter("DateBegin", db)
                , new System.Data.Services.Client.BodyOperationParameter("DateEnd", de)
                , new System.Data.Services.Client.BodyOperationParameter("InsuranceType", (byte)insType)
                , new System.Data.Services.Client.BodyOperationParameter("GovernorId", govId)
                ).FirstOrDefault();
        }

        public IQueryable<BLL.AssetValue> Context
        {
            get
            {
                return _db.Assets as IQueryable<BLL.AssetValue>;
            }
        }

        public object Create(BLL.AssetValue o)
        {
            var dto = _mapper.Map<BLL.AssetValue, AssetValue>(o);
            dto.GovernorId = o.Governor.Id;
            //dto.Governor = _db.Governors.GetById(dto.GovernorId);
            _db.AddToAssets(dto);
            _db.SaveChanges();
            return o;
        }

        public void Delete(BLL.AssetValue o)
        {
            var proxy = o as IDtoProxy;
            if (proxy != null)
            {
                _db.DeleteObject(proxy.Dto);
                _db.SaveChanges();
            }
        }

        public void Update(BLL.AssetValue o)
        {
            var proxy = o as IDtoProxy;
            
            if (proxy != null)
            {
                Guid govId = o.Governor.Id;
                var dto = proxy.Dto as AssetValue;

                if (govId != dto.GovernorId)
                {
                    dto.GovernorId = govId;
                    dto.Governor = _db.Governors.GetById(govId);
                    o.Governor = ProxyActivator.CreateProxy<Governor, BLL.Governor>(dto.Governor);
                    //_db.SetLink(dto, "Governor", gov);
                }
                
                _db.UpdateObject(dto);
                _db.SaveChanges(System.Data.Services.Client.SaveChangesOptions.ReplaceOnUpdate);
            }

            o.GovernorId = o.Governor.Id;
        }
    }
}
