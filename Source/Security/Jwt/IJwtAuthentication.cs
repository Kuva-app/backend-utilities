using System.Collections.Generic;
using System.Security.Claims;

namespace Utilities.Security.Jwt
{
    /// <summary>
    /// Interface IJwtAuthentication
    /// </summary>
    public interface IJwtAuthentication
    {
        /// <summary>
        /// Tokens the generate.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        JwtToken TokenGenerate(string name, string type, IEnumerable<Claim> claims = null);
        /// <summary>
        /// Tokens the generate.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        JwtToken TokenGenerate(ClaimsIdentity identity);
    }
}
