namespace Utilities.Security.Jwt
{
    /// <summary>
    /// Jwt Configuration
    /// </summary>
    public class JwtConfiguration
    {
        /// <summary>
        /// Gets or sets the thumb print.
        /// </summary>
        /// <value>
        /// The thumb print.
        /// </value>
        public string RsaCertificateThumbPrint { get; set; }
        /// <summary>
        /// Gets or sets the audience.
        /// </summary>
        /// <value>
        /// The audience.
        /// </value>
        public string Audience { get; set; }
        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        /// <value>
        /// The issuer.
        /// </value>
        public string Issuer { get; set; }
        /// <summary>
        /// Gets or sets the milliseconds.
        /// </summary>
        /// <value>
        /// The milliseconds.
        /// </value>
        public double Milliseconds { get; set; }
    }

}
