//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using System.Web;

using Microsoft.IdentityModel.Claims;

namespace RF.Sts.Auth
{
    /// <summary>
    /// This class represents the token format for the SimpleWebToken.
    /// </summary>
    public class SimpleWebToken : SecurityToken
    {
        string _id;        
        Uri _audienceUri;
        IEnumerable<Claim> _claims;
        string _issuer;
        DateTime _expiresOn;
        string _signature;
        string _unsignedString; 
        DateTime _validFrom;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleWebToken"/> class.
        /// This is internal contructor is only called from the <see cref="SimpleWebTokenHandler"/> when reading a token received from the wire.
        /// </summary>
        /// <param name="audienceUri">The Audience Uri of the token.</param>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="expiresOn">The expiry time of the token.</param>
        /// <param name="claims">The claims in the token.</param>
        /// <param name="signature">The signature of the token.</param>
        /// <param name="unsignedString">The serialized token without its signature.</param>
        internal SimpleWebToken( Uri audienceUri, string issuer, DateTime expiresOn, List<Claim> claims, string signature, string unsignedString )
            : this(audienceUri, issuer, expiresOn, claims)
        {
            _signature = signature;
            _unsignedString = unsignedString;
        }

        private SimpleWebToken(Uri audienceUri, string issuer, DateTime expiresOn, List<Claim> claims)
            : this()
        {
            _audienceUri = audienceUri;
            _issuer = issuer;
            _expiresOn = expiresOn;
            _claims = claims;
        }

        public SimpleWebToken(Uri audienceUri, string issuer, DateTime expiresOn, List<Claim> claims, string key)
            : this(audienceUri, issuer, expiresOn, claims)
        {
            _unsignedString = this.ToPostString();
            _signature = this.Sign(key);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleWebToken"/> class.
        /// </summary>
        private SimpleWebToken()
        {
            _validFrom = SwtConst.BaseTime;
            _id = null;
        }

        /// <summary>
        /// Gets the Id of the token.
        /// </summary>
        /// <value>The Id of the token.</value>
        public override string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the keys associated with this token.
        /// </summary>
        /// <value>The keys associated with this token.</value>
        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get { return new ReadOnlyCollection<SecurityKey>( new List<SecurityKey>() ); }
        }
        
        /// <summary>
        /// Gets the time from when the token is valid.
        /// </summary>
        /// <value>The time from when the token is valid.</value>
        public override DateTime ValidFrom
        {           
            get { return _validFrom; }            
        }

        /// <summary>
        /// Gets the time when the token expires.
        /// </summary>
        /// <value>The time upto which the token is valid.</value>
        public override DateTime ValidTo
        {           
            get { return _expiresOn; }           
        }

        /// <summary>
        /// Gets the AudienceUri for the token.
        /// </summary>
        /// <value>The audience Uri of the token.</value>
        public Uri AudienceUri
        {
            get { return _audienceUri; }            
        }

        /// <summary>
        /// Gets the Issuer for the token.
        /// </summary>
        /// <value>The issuer for the token.</value>
        public string Issuer
        {
            get { return _issuer; }
        }

        /// <summary>
        /// Gets the Claims in the token.
        /// </summary>
        /// <value>The Claims in the token.</value>
        public IEnumerable<Claim> Claims
        {
            get { return _claims; }            
        }

        /// <summary>
        /// Verifies the signature of the incoming token.
        /// </summary>
        /// <param name="key">The key used for signing.</param>
        /// <returns>true if the signatures match, false otherwise.</returns>
        public bool SignVerify(byte[] key)
        {
            if ( key == null )
            {
                throw new ArgumentNullException( "key" );
            }

            if ( _signature == null || _unsignedString == null )
            {
                throw new InvalidOperationException( "Token has never been signed" );
            }

            string verifySignature;

            using ( HMACSHA256 signatureAlgorithm = new HMACSHA256( key ) )
            {
                verifySignature = Convert.ToBase64String( signatureAlgorithm.ComputeHash( Encoding.ASCII.GetBytes( _unsignedString ) ) );
            }

            if ( string.CompareOrdinal( verifySignature, _signature ) == 0 )
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0}&{1}={2}", this._unsignedString, SwtConst.Digest256Label, this._signature);
        }

        private string ToPostString()
        {
            StringBuilder content = new StringBuilder();
            content.AppendFormat("{0}={1}", SwtConst.IssuerLabel, this.Issuer);
            foreach (var claim in this.Claims)
            {
                content.AppendFormat("&{0}={1}", claim.ClaimType, claim.Value);
            }

            TimeSpan ts = this.ValidTo.ToUniversalTime() - SwtConst.BaseTime;
            content.AppendFormat("&{0}={1}", SwtConst.ExpiresOnLabel, Convert.ToUInt64(ts.TotalSeconds));
            content.AppendFormat("&{0}={1}", SwtConst.AudienceLabel, this.AudienceUri);

            return content.ToString();
        }

        private string Sign(string key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(key)))
            {
                byte[] signatureBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(this._unsignedString));
                string signature = System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(signatureBytes));
                return signature;
            }
        }
    }    
}