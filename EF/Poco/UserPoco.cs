using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EF.Poco
{
	public class UserPoco : BasePoco
	{
		public Guid PersonId { get; set; }
		public string Name { get; set; }
		public bool IsDisabled { get; set; }
		public string EMail { get; set; }
		public DateTime? ValidDate { get; set; }
		public string MappedSamAccountName { get; set; }
		public DateTime? LastPasswordChangeDate { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] Salt { get; set; }

        public virtual ICollection<PolicyGroupPoco> Groups { get; set; }
        public virtual ICollection<PolicyPoco> Policies { get; set; }
	}
}
