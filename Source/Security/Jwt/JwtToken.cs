using System;

namespace Utilities.Security.Jwt
{
    /// <summary>
    /// Jwt Token
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// Gets or sets the create at.
        /// </summary>
        /// <value>
        /// The create at.
        /// </value>
        public DateTime CreateAt { get; set; }
        /// <summary>
        /// Gets or sets the expire at.
        /// </summary>
        /// <value>
        /// The expire at.
        /// </value>
        public DateTime ExpireAt { get; set; }
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string AccessToken { get; set; }
        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        /// <value>
        /// The scheme.
        /// </value>
        public string Scheme { get; set; }
    }
}
