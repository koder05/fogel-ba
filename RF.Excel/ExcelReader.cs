using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace RF.Excel
{
    public class ExcelReader : MarshalByRefObject, IExcelReader
    {
        private const string connXlsStringFormat = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=\"{0}\"; Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;\"";
        private const string connXlsxStringFormat = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\"{0}\"; Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1;\"";

        private string GetConnectionString(string xlsFilePath)
        {
            if (xlsFilePath.EndsWith("xlsx", StringComparison.InvariantCultureIgnoreCase))
                return string.Format(connXlsxStringFormat, xlsFilePath);
            if (xlsFilePath.EndsWith("xls", StringComparison.InvariantCultureIgnoreCase))
                return string.Format(connXlsStringFormat, xlsFilePath);

            throw new InvalidOperationException("Неизвестный формат файла Excel для импорта данных.");
        }

        public DataTable ExecuteSelect(string xlsFilePath, string selectQuery)
        {
            if (string.IsNullOrEmpty(xlsFilePath))
                throw new ArgumentNullException("xlsFilePath");

            if (string.IsNullOrEmpty(selectQuery))
                throw new ArgumentNullException("selectQuery");

            try
            {
                string excelConnString = GetConnectionString(xlsFilePath);
                //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\myexcelfile.xlsx;Extended Properties="Excel 12.0;HDR=YES;IMEX=1";

                using (OleDbConnection excelCon = new OleDbConnection(excelConnString))
                {
                    excelCon.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(selectQuery, excelCon);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new ExcelReaderException(string.Format("Невозможно загрузить MS Excel файл. " + Environment.NewLine + "Ошибка: {0}", ex.Message), ex);
            }
        }

        public IEnumerable<string> GetSheetNames(string xlsFilePath)
        {
            string[] workSheetNames;
            string excelConnString = GetConnectionString(xlsFilePath);

            using (var connection = new OleDbConnection(excelConnString))
            {
                connection.Open();
                DataTable dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables_Info, null);
                workSheetNames = new string[dt.Rows.Count];
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if ((decimal?)row["CARDINALITY"] == 0)
                    {
                        workSheetNames[i] = row["TABLE_NAME"].ToString().Trim('\'');
                        i++;
                    }
                }
            }

            return workSheetNames.Where(s => !string.IsNullOrEmpty(s));
        }

        public static IExcelReader GetReader()
        {
            return new ExcelReader();
        }

        public static IExcelReader GetRemoteReader()
        {
            string uri = ConfigurationManager.AppSettings["ExcelReaderUri"];
            if (string.IsNullOrEmpty(uri))
                throw new Exception("Excel reader URI must be configured in appSettings section by key \"ExcelReaderUri\".");

            IExcelReader reader = (IExcelReader)Activator.GetObject(typeof(IExcelReader), uri);
            return reader;
        }
    }
}