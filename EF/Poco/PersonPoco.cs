using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace EF.Poco
{
	public class PersonPoco : BasePoco
	{
		public string LN { get; set; }
		public string FN { get; set; }
		public string SN { get; set; }
        public virtual ICollection<ZlPoco> Zls { get; set; }
	}

    class PersonCfg : EntityTypeConfiguration<PersonPoco>
    {
        internal PersonCfg()
        {
            this.ToTable("tbl_Person");
            this.HasKey(m => m.Id).Property(m => m.Id).HasColumnName("ID_Person");
            this.Property(m => m.LN).HasColumnName("LastName");
            this.Property(m => m.FN).HasColumnName("FirstName");
            this.Property(m => m.SN).HasColumnName("MiddleName");

            this.HasMany(m => m.Zls).WithMany(zl => zl.Persons).Map(m => m.MapLeftKey("ID_Person").MapRightKey("KodPS").ToTable("tbl_PersonToPS"));
        }
    }
}
