using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Sts.Auth
{
    public static class OAuthError
    {
        public const string INVALID_REQUEST = "invalid_request";
        public const string INVALID_CLIENT = "invalid_client";
        public const string INVALID_GRANT = "invalid_grant";
        public const string UNAUTHORIZED_CLIENT = "unauthorized_client";
        public const string UNSUPPORTED_GRANT_TYPE = "unsupported_grant_type";
        public const string INVALID_SCOPE = "invalid_scope";
    }
}
