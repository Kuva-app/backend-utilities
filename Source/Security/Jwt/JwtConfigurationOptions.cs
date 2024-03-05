namespace Utilities.Security.Jwt
{
    /// <summary>
    /// Jwt Configuration Options
    /// </summary>
    public sealed class JwtConfigurationOptions
    {
        /// <summary>
        /// Gets or sets the signing configuration.
        /// </summary>
        /// <value>
        /// The signing configuration.
        /// </value>
        public RsaSigningConfiguration SigningConfiguration { get; set; }
    }
}
