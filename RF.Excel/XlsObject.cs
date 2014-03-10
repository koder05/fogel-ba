using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace RF.Excel
{
    public abstract class XlsObject<T> where T : class
	{
        protected string excelFileName { get; private set; }
        protected string dataSheetName { get; private set; }
		protected IExcelReader ImageReader { get; private set; }

        public XlsObject(byte[] xlsImage, string dataSheet)
		{
            if (xlsImage == null)
                throw new InvalidOperationException("Попытка чтения неинициализированного образа Excel.");

            if (xlsImage.Length == 0)
                throw new InvalidOperationException("Попытка чтения пустого образа Excel.");

            string xlsFilePath = Path.GetTempFileName();

            try
            {
                File.WriteAllBytes(excelFileName, xlsImage);
            }
            catch (Exception ex)
            {
                if (File.Exists(xlsFilePath))
                    File.Delete(xlsFilePath);
                throw new ExcelReaderException(string.Format("Невозможно загрузить MS Excel файл. " + Environment.NewLine + "Ошибка: {0}", ex.Message), ex);
            }

            Init(xlsFilePath, dataSheet);
		}

        public XlsObject(string xlsFilePath, string dataSheet)
        {
            Init(xlsFilePath, dataSheet);
        }

        private void Init(string xlsFilePath, string dataSheet)
        {
            excelFileName = xlsFilePath;
            ImageReader = ExcelReader.GetReader();
            dataSheetName = dataSheet;
        }

        protected abstract IEnumerable<T> ParseData(DataTable data);

        protected abstract string GetSelectExpr();

		public IEnumerable<T> SelectAll()
		{
            //(ImageReader as ExcelReader).GetSheetNames(excelFileName);

            DataTable data = ImageReader.ExecuteSelect(excelFileName, GetSelectExpr());
			return ParseData(data);
		}
	}
}
