using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace EF.Poco
{
    class UserCfg : EntityTypeConfiguration<UserPoco>
    {
        internal UserCfg()
        {
            this.ToTable("tbl_User", "Security");
            this.HasKey(m => m.Id).Property(m => m.Id).HasColumnName("ID_User");
            this.Property(m => m.EMail).HasColumnName("EMail");
            this.Property(m => m.IsDisabled).HasColumnName("IsDisabled");
            this.Property(m => m.LastPasswordChangeDate).HasColumnName("LastPasswordChangeDate");
            this.Property(m => m.MappedSamAccountName).HasColumnName("MappedSamAccountName");
            this.Property(m => m.Name).HasColumnName("UserName");
            this.Property(m => m.PersonId).HasColumnName("ID_Person");
            this.Property(m => m.ValidDate).HasColumnName("ValidDate");
            this.Property(m => m.PasswordHash).HasColumnName("PasswordHash");
            this.Property(m => m.Salt).HasColumnName("Salt");

            this.HasMany(m => m.Groups).WithMany(g => g.Members).Map(m => m.MapLeftKey("ID_User").MapRightKey("ID_Group").ToTable("tbl_GroupMember", "Security"));
            this.HasMany(m => m.Policies).WithMany(p => p.Users).Map(m => m.MapLeftKey("ID_User").MapRightKey("ID_Policy").ToTable("tbl_PolicyUserOwner", "Security"));
        }
    }
}
