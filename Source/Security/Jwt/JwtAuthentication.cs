using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Utilities.Security.Jwt
{
    /// <summary>
    /// Jwt Authentication
    /// </summary>
    /// <seealso cref="IJwtAuthentication" />
    public class JwtAuthentication : IJwtAuthentication
    {

        /// <summary>
        /// The default expiration milliseconds
        /// </summary>
        private const double DefaultExpirationMilliseconds = 600_000_000;

        /// <summary>
        /// The signing configuration
        /// </summary>
        private readonly RsaSigningConfiguration _signingConfiguration;
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly JwtConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtAuthentication"/> class.
        /// </summary>
        /// <param name="signingConfiguration">The signing configuration.</param>
        /// <param name="configuration">The configuration.</param>
        public JwtAuthentication(RsaSigningConfiguration signingConfiguration, JwtConfiguration configuration)
        {
            _signingConfiguration = signingConfiguration;
            _configuration = configuration;
        }
        /// <summary>
        /// Tokens the generate.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        public JwtToken TokenGenerate(string name, string type, IEnumerable<Claim> claims = null)
        {
            var identity = new ClaimsIdentity(new GenericIdentity(name,
                                                                  type),
                                                                  claims ??
            new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, name)
            });
            return TokenGenerate(identity);
        }
        /// <summary>
        /// Tokens the generate.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        public JwtToken TokenGenerate(ClaimsIdentity identity)
        {
            var jwtToken = new JwtToken
            {
                CreateAt = DateTime.Now
            };
            try
            {
                jwtToken.ExpireAt = jwtToken.CreateAt.AddMilliseconds(_configuration?.Milliseconds ?? DefaultExpirationMilliseconds);
                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _configuration?.Issuer,
                    Audience = _configuration?.Audience,
                    SigningCredentials = _signingConfiguration.SigningCredentials,
                    Subject = identity,
                    NotBefore = jwtToken.CreateAt,
                    Expires = jwtToken.ExpireAt
                });
                jwtToken.AccessToken = handler.WriteToken(securityToken);
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(e);
#endif
                return null;
            }
            return jwtToken;
        }
    }
}
