using System;
using System.Collections.Generic;
using System.Data;

namespace RF.Excel
{
	public interface IExcelReader
	{
        DataTable ExecuteSelect(string xlsFilePath, string selectQuery);
        IEnumerable<string> GetSheetNames(string xlsFilePath);
	}
}