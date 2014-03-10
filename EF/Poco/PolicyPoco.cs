using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EF.Poco
{
    public class PolicyPoco : BasePoco
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual ICollection<UserPoco> Users { get; set; }
        public virtual ICollection<PolicyGroupPoco> Groups { get; set; }
    }
}
