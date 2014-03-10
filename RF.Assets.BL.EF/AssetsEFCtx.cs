using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

using RF.BL.Model;

namespace EF
{
    public class AssetsEFCtx : EFContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Governor> Governors { get; set; }
        public DbSet<WorkCalendar> Holidays { get; set; }
        public DbSet<AssetValue> Assets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().ToTable("tbl_Company");
            modelBuilder.Entity<Company>().HasKey(m => m.Id).Property(m => m.Id).HasColumnName("ID_Company");
            modelBuilder.Entity<Company>().Property(m => m.Name).HasColumnName("Name");
            modelBuilder.Entity<Company>().Property(m => m.lawFormValue).HasColumnName("LawFormType");
            modelBuilder.Entity<Company>().Ignore(m => m.LawForm);

            modelBuilder.Entity<Governor>().ToTable("tbl_Governor");
            modelBuilder.Entity<Governor>().HasKey(m => m.Id).Property(m => m.Id).HasColumnName("ID_Governor");
            modelBuilder.Entity<Governor>().Property(m => m.CompanyId).HasColumnName("ID_Company");
            modelBuilder.Entity<Governor>().HasRequired(m => m.Company).WithMany().HasForeignKey(m => m.CompanyId);
            modelBuilder.Entity<Governor>().Property(m => m.ShortName).HasColumnName("ShortName");

            modelBuilder.Entity<WorkCalendar>().ToTable("tbl_Holidays");
            modelBuilder.Entity<WorkCalendar>().HasKey(m => m.Id).Property(m => m.Id).HasColumnName("ID_Holiday");
            modelBuilder.Entity<WorkCalendar>().Property(m => m.BankDate).HasColumnName("BankDate");
            modelBuilder.Entity<WorkCalendar>().Property(m => m.BankDate_Key).HasColumnName("BankDate_Key");
            modelBuilder.Entity<WorkCalendar>().Property(m => m.Comment).HasColumnName("Comment");
            modelBuilder.Entity<WorkCalendar>().Property(m => m.IsWorkingDay).HasColumnName("IsWorkingDay");
            modelBuilder.Entity<WorkCalendar>().Property(m => m.Date).HasColumnName("Date");
            modelBuilder.Entity<WorkCalendar>().Ignore(m => m.DayType);

            modelBuilder.Entity<AssetValue>().ToTable("tbl_AssetsValue");
            modelBuilder.Entity<AssetValue>().HasKey(m => m.Id).Property(m => m.Id).HasColumnName("ID_AssetsValue");
            modelBuilder.Entity<AssetValue>().Property(m => m.TakingDate).HasColumnName("TakingDate");
            modelBuilder.Entity<AssetValue>().Property(m => m.Value).HasColumnName("Value");
            modelBuilder.Entity<AssetValue>().Property(m => m.CashFlow).HasColumnName("CashFlow");
            modelBuilder.Entity<AssetValue>().Property(m => m.InsuranceTypeValue).HasColumnName("InsuranceType");
            modelBuilder.Entity<AssetValue>().Property(m => m.GovernorId).HasColumnName("ID_Governor");
            modelBuilder.Entity<AssetValue>().HasRequired(m => m.Governor).WithMany().HasForeignKey(m => m.GovernorId);
            modelBuilder.Entity<AssetValue>().Ignore(m => m.InsuranceType);
        }

        public XmlDocument SqlQueryXml(string query, string rootName, string rowName, params SqlParameter[] parameters)
        {
            query = string.Format("{0} for xml raw ('{1}'), root ('{2}')", query, string.IsNullOrEmpty(rowName) ? "row" : rowName, string.IsNullOrEmpty(rootName) ? "root" : rootName);

            var ret = this.Database.SqlQuery<string>(query, parameters).ToList();
            StringBuilder sb = new StringBuilder(ret.Count * 2048);
            ret.ForEach(s => sb.Append(s));
            string xml = sb.ToString();

            XmlDocument doc = new XmlDocument();

            if (string.IsNullOrEmpty(xml) == false)
                doc.LoadXml(xml);

            return doc;
        }
    }
}
