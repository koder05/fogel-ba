using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace EF.Poco
{
    class PolicyCfg : EntityTypeConfiguration<PolicyPoco>
    {
        internal PolicyCfg()
        {
            this.ToTable("Security.tbl_Policy");
            this.HasKey(m => m.Id).Property(m => m.Id).HasColumnName("ID_Policy");
            this.Property(m => m.Name).HasColumnName("PolicyName");
            this.Property(m => m.Description).HasColumnName("Description");
            this.Property(m => m.CategoryId).HasColumnName("ID_Category");

            this.HasMany(m => m.Groups).WithMany(g => g.Policies).Map(m => m.MapLeftKey("ID_Policy").MapRightKey("ID_Group").ToTable("tbl_PolicyGroupOwner", "Security"));
            this.HasMany(m => m.Users).WithMany(u => u.Policies).Map(m => m.MapLeftKey("ID_Policy").MapRightKey("ID_User").ToTable("tbl_PolicyUserOwner", "Security"));
        }
    }

    class PolicyGroupCfg : EntityTypeConfiguration<PolicyGroupPoco>
    {
        internal PolicyGroupCfg()
        {
            this.ToTable("Security.tbl_Group");
            this.HasKey(m => m.Id).Property(m => m.Id).HasColumnName("ID_Group");
            this.Property(m => m.Name).HasColumnName("GroupName");
            this.Property(m => m.Description).HasColumnName("Description");
            this.Property(m => m.IsDisabled).HasColumnName("IsDisabled");
            this.Property(m => m.IsSystem).HasColumnName("IsSystem");

            this.HasMany(m => m.Members).WithMany(u => u.Groups).Map(m => m.MapLeftKey("ID_Group").MapRightKey("ID_User").ToTable("tbl_GroupMember", "Security"));
            this.HasMany(m => m.Policies).WithMany(p => p.Groups).Map(m => m.MapLeftKey("ID_Group").MapRightKey("ID_Policy").ToTable("tbl_PolicyGroupOwner", "Security"));
        }
    }
}
