using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using RF.Excel;
using RF.BL.Model;
using RF.BL.Model.Enums;

namespace RF.BL.Excel
{
    /// <summary>
    /// Стоимость чистых активов (СЧА) : формат с ДДC
    /// </summary>
    public class AssetsCashXlsObject : XlsObject<AssetValue>
    {
        private static int _ExcelStartRowNumber = 2;
        private static string _ExcelEndColumnName = "Z";

        private IEnumerable<Governor> _governors;
        private InsuranceType _insType;

        public AssetsCashXlsObject(IEnumerable<Governor> governors, InsuranceType insType, string xlsFilePath, string dataSheet)
            : base(xlsFilePath, dataSheet)
        {
            _governors = governors;
            _insType = insType;
        }

        protected override IEnumerable<AssetValue> ParseData(DataTable data)
        {
            if (data == null || data.Columns.Count < 4)
                throw new InvalidOperationException("Несовместимый формат файла Excel для импорта данных по ДДС СЧА.");

            string currentsgov = string.Empty;
            Governor currentgov = null;

            DataView dv = data.DefaultView;
            foreach (DataRowView r in dv)
            {
                //получаем дату из первой колонки
                string sdt = Convert.ToString(r[0]);
                DateTime dt = DateTime.Today;
                if (false == string.IsNullOrEmpty(sdt) && DateTime.TryParse(sdt, out dt))
                {
                    //получаем УК из 4ой колонки
                    string sgov = Convert.ToString(r[3]).Trim();

                    if (false == string.IsNullOrEmpty(sgov))
                    {
                        if (currentsgov != sgov)
                        {
                            currentsgov = sgov;
                            currentgov = _governors.FirstOrDefault(g => currentsgov.Contains(g.ShortName));
                        }

                        if (currentgov != null)
                        {
                            //получаем цифирь из второй колонки
                            string sval = Convert.ToString(r[1]);
                            decimal val = 0;
                            if (decimal.TryParse(sval, out val))
                            {
                                yield return new AssetValue() { TakingDate = dt, CashFlow = val, Governor = currentgov, GovernorId = currentgov.Id, InsuranceType = _insType };
                            }
                        }
                    }
                }
            }
        }

        protected override string GetSelectExpr()
        {
            return string.Format("select * from [{3}A{0}:{1}{2}]", _ExcelStartRowNumber, _ExcelEndColumnName, _ExcelStartRowNumber + 1 + 999, dataSheetName);
        }
    }
}
