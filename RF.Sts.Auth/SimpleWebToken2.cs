using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Tokens;
using System.IdentityModel.Claims;
using System.Security.Cryptography;

using Microsoft.IdentityModel.Protocols.OAuth;
using Microsoft.IdentityModel.Protocols.OAuth.Client;


namespace RF.Sts.Auth
{
    public class SimpleWebToken2 
    {
        private static readonly TimeSpan lifeTime = new TimeSpan(0, 2, 0);
        private static readonly DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
        private IList<Claim> claims = new List<Claim>();
        private byte[] keyBytes = null;

        public SimpleWebToken2(string key)
        {
            TimeSpan ts = DateTime.UtcNow - epochStart + lifeTime;
            this.ExpiresOn = Convert.ToUInt64(ts.TotalSeconds);

            var securityKey = new InMemorySymmetricSecurityKey(Convert.FromBase64String(key));
            keyBytes = securityKey.GetSymmetricKey();
        }

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public byte[] Signature { get; set; }
        public ulong ExpiresOn { get; private set; }

        public IList<Claim> Claims
        {
            get
            {
                return this.claims;
            }
        }

        public void AddClaim(string type, string value, string right)
        {
            claims.Add(new Claim(type, value, right));
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();

            content.Append("Issuer=").Append(this.Issuer);


            for (int i = 0; i < this.claims.Count; i++)
            {
                var claim = claims[i];
                if (i == 0)
                    content.Append('&').Append("Claims").Append('=');
                content.AppendFormat("{0}:{1}:{2}", claim.ClaimType, claim.Resource, claim.Right);
                if (i < this.claims.Count - 1)
                    content.Append(',');
            }

            content.Append("&ExpiresOn=").Append(this.ExpiresOn);

            if (!string.IsNullOrWhiteSpace(this.Audience))
            {
                content.Append("&Audience=").Append(this.Audience);
            }

            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(content.ToString()));

                string signature = System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(signatureBytes));

                content.Append("&HMACSHA256=").Append(signature);
            }

            return content.ToString();
        }
    }
}
