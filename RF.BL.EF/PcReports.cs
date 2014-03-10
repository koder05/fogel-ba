using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;

using EF;
using RF.BL.PC;
using RF.Common;

namespace RF.BL.EF
{
    public class PcReports : IPcReports
    {
        private EFContext _db;
        public PcReports(EFContext db)
        {
            _db = db;
        }

        public IEnumerable<DdsReport> GetDdsReport(Guid personId)
        {
            var parPersonId = new SqlParameter("@personId", /*new Guid("9882C3FA-9A4B-4E21-B4E4-1B36CFB5CF4D")*/personId);
            var ret = _db.Database.SqlQuery<string>("exec dbo.GetZlDds @personId", parPersonId).ToList();
            StringBuilder sb = new StringBuilder(ret.Count * 2048);  
            ret.ForEach(s => sb.Append(s));
            string xml = sb.ToString(); 
            
            XmlDocument doc = new XmlDocument();

            if (string.IsNullOrEmpty(xml) == false)
                doc.LoadXml(xml);

            foreach (XmlNode el in doc.SelectNodes("//row"))
                yield return Utils.GetObjectFromXml<DdsReport>(el);
        }
    }
}
