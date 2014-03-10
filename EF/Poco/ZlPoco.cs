using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace EF.Poco
{
    public class ZlPoco : BasePoco
    {
        public int KodPs { get; set; }
        public string InsCert { get; set; }
        public string LN { get; set; }
        public string FN { get; set; }
        public string SN { get; set; }
        public string GenderName { get; set; }
        public virtual ICollection<PersonPoco> Persons { get; set; }
    }

    class ZlCfg : EntityTypeConfiguration<ZlPoco>
    {
        internal ZlCfg()
        {
            this.ToTable("СписокПринятыхЛиц");
            this.HasKey(m => m.KodPs).Property(m => m.KodPs).HasColumnName("KodPS");
            this.Ignore(m => m.Id);
            this.Property(m => m.InsCert).HasColumnName("SS_PFR");
            this.Property(m => m.LN).HasColumnName("Familiya");
            this.Property(m => m.FN).HasColumnName("Imya");
            this.Property(m => m.SN).HasColumnName("Otchestvo");
            this.Property(m => m.GenderName).HasColumnName("Pol");

            this.HasMany(m => m.Persons).WithMany(p => p.Zls).Map(m => m.MapLeftKey("KodPS").MapRightKey("ID_Person").ToTable("tbl_PersonToPS"));
        }
    }
}
