using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Text;
using Microsoft.IdentityModel.Claims;

using RF.Sts.Auth;
using RF.Sts.Auth.Configuration;

namespace RF.WinApp.Svc.Controllers
{
    [Authorize]
    public class IssueController : ApiController
    {
        // POST api/issue
        public HttpResponseMessage Post(TokenRequest rst)
        {
            Uri scope = rst.Scope;

            if (scope == null)
            {
                return Request.CreateResponse<TokenResponse>(HttpStatusCode.BadRequest, new TokenResponse() { Error = OAuthError.INVALID_REQUEST });
            }

            string key = OAuthConfiguration.Configuration.StsSettings.SymmetricKey;
            TimeSpan lifeTime = new TimeSpan(0, 0, OAuthConfiguration.Configuration.StsSettings.TokenLifeTimeInSec); 

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, this.User.Identity.Name));
            claims.Add(new Claim(ClaimTypes.Role, "AssetsServiceUser"));
            claims.Add(new Claim(ClaimTypes.Role, "Developer"));
            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));

            SimpleWebToken token = new SimpleWebToken(scope, OAuthConfiguration.Configuration.StsSettings.IssuerUri.ToString(), DateTime.UtcNow + lifeTime, claims, key);

            var tokenResponse = new TokenResponse() { AccessToken = token.ToString(), TokenType = "bearer", ExpiresIn = 600 };
            return Request.CreateResponse<TokenResponse>(HttpStatusCode.OK, tokenResponse);
        }
    }
}
