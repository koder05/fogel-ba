using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using RF.Common;
using RF.Geo.BL;
using RF.Geo.Sql;

namespace RF.Geo.Parsers
{
    public class AddressTemplate
    {
        private DataRowView View { get; set; }

        public const string TableName = "Geo.tbl_ParserTemplates";

        public static class ColumnNames
        {
            public const string ID = "ID_Node";
            public const string ParentID = "ID_ParentNode";
            public const string TemplateID = "ID_Template";
            public const string Level = "NodeLevel";
        }

        #region Constructors

        public AddressTemplate(int id, int parentID, int templateID, int level)
        {
            View = Init(id, parentID, templateID, level);
        }

        public AddressTemplate(DataRowView row)
        {
            View = row;
        }

        private DataRowView Init(int id, int parentID, int templateID, int level)
        {
            DataTable t = new DataTable();
            t.Columns.Add(ColumnNames.ID, typeof(int));
            t.Columns.Add(ColumnNames.ParentID, typeof(int));
            t.Columns.Add(ColumnNames.TemplateID, typeof(int));
            t.Columns.Add(ColumnNames.Level, typeof(int));
            DataRow r = t.NewRow();
            r[ColumnNames.ID] = id;
            r[ColumnNames.ParentID] = parentID;
            r[ColumnNames.TemplateID] = templateID;
            r[ColumnNames.Level] = level;
            t.Rows.Add(r);
            return t.DefaultView[0];
        }

        #endregion

        #region Properties

        private Dictionary<string, int> propValues = new Dictionary<string, int>();

        private int GetProperty(string name)
        {
            if (false == propValues.ContainsKey(name))
                propValues.Add(name, this.View[name] == DBNull.Value ? -1 : Convert.ToInt32(this.View[name]));
            return propValues[name];
        }

        public int ID
        {
            get
            {
                return GetProperty(ColumnNames.ID);
            }
        }

        public int ParentID
        {
            get
            {
                return GetProperty(ColumnNames.ParentID);
            }
        }

        public int TemplateID
        {
            get
            {
                return GetProperty(ColumnNames.TemplateID);
            }
        }

        public int Level
        {
            get
            {
                return GetProperty(ColumnNames.Level);
            }
        }

        #endregion

        #region Static

        private static SqlCommand GetCmd()
        {
            SqlCommand cmd = DbHelper.CreateCommand("select * from tblAddrParserTemplates");
            return cmd;
        }

        public static List<AddressTemplate> GetList()
        {
            List<AddressTemplate> list = new List<AddressTemplate>();
            foreach (DataRowView row in DbHelper.SelectDataView(GetCmd()))
            {
                list.Add(new AddressTemplate(row));
            }
            return list;
        }

        #endregion
    }
}
