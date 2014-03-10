using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Sts.Auth
{
    public static class SwtConst
    {
        public const string AudienceLabel = "Audience";
        public const string ExpiresOnLabel = "ExpiresOn";
        public const string IssuerLabel = "Issuer";
        public const string Digest256Label = "HMACSHA256";
        public const string DefaultNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string AcsNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public static DateTime BaseTime = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
        //public static string SymmetricSignatureKey = SamplesConfiguration.RelyingPartySigningKey;
    }
}
