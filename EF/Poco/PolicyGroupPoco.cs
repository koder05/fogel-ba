using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EF.Poco
{
    public class PolicyGroupPoco : BasePoco
    {
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsSystem { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserPoco> Members { get; set; }
        public virtual ICollection<PolicyPoco> Policies { get; set; }
    }
}
