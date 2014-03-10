using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations; 

namespace EF.Poco
{
	public class PswChReqPoco : BasePoco
	{
		public DateTime CreationDate { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public Guid? UserID { get; set; }
		public Guid? PersonID { get; set; }
		public string UserEmail { get; set; }
		public string UserIPAddress { get; set; }
		public string LoginName { get; set; }
		public byte EntryType { get; set; }
		public bool IsActive { get; set; }
	}

	class PswChReqCfg : EntityTypeConfiguration<PswChReqPoco>
	{
		internal PswChReqCfg()
		{
			this.ToTable("Security.tbl_UserChangePassword");
			this.HasKey(m => m.Id).Property(m => m.Id).HasColumnName("ID_ChangePassword").IsRequired().HasColumnType("uniqueidentifier");  
			this.Property(m => m.CreationDate).HasColumnName("EntryDate");
			this.Property(m => m.EntryType).HasColumnName("EntryType");
			this.Property(m => m.ExpirationDate).HasColumnName("ExpirationDate");
			this.Property(m => m.LoginName).HasColumnName("LoginName");
			this.Property(m => m.PersonID).HasColumnName("ID_Person");
			this.Property(m => m.UserEmail).HasColumnName("RecipientEmail");
			this.Property(m => m.UserID).HasColumnName("ID_User");
			this.Property(m => m.UserIPAddress).HasColumnName("IPAddress");
			this.Property(m => m.IsActive).HasColumnName("RequestEnabled");
		}
	}
}
