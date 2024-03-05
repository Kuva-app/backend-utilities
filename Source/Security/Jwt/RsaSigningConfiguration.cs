using System;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Utilities.Security.Cryptography;

namespace Utilities.Security.Jwt
{
    /// <summary>
    /// RSA Signing configuration
    /// </summary>
    public class RsaSigningConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RsaSigningConfiguration"/> class.
        /// </summary>
        public RsaSigningConfiguration()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaSigningConfiguration"/> class.
        /// </summary>
        /// <param name="thumbPrint">The thumb print.</param>
        /// <exception cref="ArgumentNullException">thumbPrint</exception>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public RsaSigningConfiguration(string thumbPrint)
        {
            if (string.IsNullOrEmpty(thumbPrint))
                throw new ArgumentNullException(nameof(thumbPrint));
            var rsaCrypto = new RsaCryptography();
            var cert = rsaCrypto.GetPrivateKeyCertBy(thumbPrint);
            if (cert == null)
                throw new NullReferenceException();
            var cert2 = new X509Certificate2(cert);
            if (cert2 == null)
                throw new NullReferenceException();
            using (cert2)
            using (var provider = cert2.PublicKey.Key as RSACryptoServiceProvider)
            {
                if (provider != null) Key = new RsaSecurityKey(provider.ExportParameters(true));
            }
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public SecurityKey Key { get; }
        /// <summary>
        /// Gets the signing credentials.
        /// </summary>
        /// <value>
        /// The signing credentials.
        /// </value>
        public SigningCredentials SigningCredentials { get; }
    }
}
