using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using RF.Excel;
using RF.BL.Model;
using RF.BL.Model.Enums;

namespace RF.BL.Excel
{
	/// <summary>
	/// Стоимость чистых активов (СЧА) : формат со СЧА
	/// </summary>
	public class AssetsValXlsObject : XlsObject<AssetValue>
	{
		private static int _ExcelStartRowNumber = 1;
		private static string _ExcelEndColumnName = "Z";

        private IEnumerable<Governor> _governors;
        private InsuranceType _insType;

        public AssetsValXlsObject(IEnumerable<Governor> governors, InsuranceType insType, string xlsFilePath, string dataSheet) 
            : base (xlsFilePath, dataSheet )
        {
            _governors = governors;
            _insType = insType;
        }

        protected override IEnumerable<AssetValue> ParseData(DataTable data)
		{
			if (data == null || data.Columns.Count < 3)
				throw new InvalidOperationException("Несовместимый формат файла Excel для импорта данных по СЧА.");
			
			DataView dv = data.DefaultView;
			foreach (DataRowView r in dv)
			{
				//получаем дату из первой колонки
				string sdt = Convert.ToString(r[0]);
				DateTime dt = DateTime.Today;
				if (false == string.IsNullOrEmpty(sdt) && DateTime.TryParse(sdt, out dt))
				{
					//получаем УК начиная с 3ей колонки
					for (int i = 2; i < data.Columns.Count; i++)
					{
						string sgov = data.Columns[i].ColumnName;
                        Governor gov = _governors.FirstOrDefault(g => sgov.Contains(g.ShortName));
						if (gov != null)
						{
							//получаем цифирь
							string sval = Convert.ToString(r[i]);
							decimal val = 0;
							decimal.TryParse(sval, out val);
                            yield return new AssetValue() { TakingDate = dt, Value = val, Governor = gov, GovernorId = gov.Id, InsuranceType = _insType };
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
