using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RF.Sts.Auth
{
    [DataContract]
    public class TokenRequest
    {
        [DataMember(Name = "scope", EmitDefaultValue = false)]
        public Uri Scope { get; set; }
    }
}
