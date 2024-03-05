using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Utilities.Security.Jwt
{
    /// <summary>
    /// Jwt Startup
    /// </summary>
    public static class JwtStartUp
    {
        /// <summary>
        /// Adds the kuva JWT.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="jwtConfigurationSection">The JWT configuration section.</param>
        public static void AddKuvaJwt(this IServiceCollection service,
                                      IConfigurationSection jwtConfigurationSection)
        {
            var configuration = new JwtConfiguration();
            new ConfigureFromConfigurationOptions<JwtConfiguration>(jwtConfigurationSection)
                .Configure(configuration);
            service.AddSingleton(configuration);
            var signinConfiguration = string.IsNullOrEmpty(configuration.RsaCertificateThumbPrint)
                ? new RsaSigningConfiguration()
                : new RsaSigningConfiguration(configuration.RsaCertificateThumbPrint);
            service.AddSingleton(signinConfiguration);
            service.AddAuthentication(_ =>
                {
                    _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(_ =>
                {
                    var paramValidation = _.TokenValidationParameters;
                    paramValidation.IssuerSigningKey = signinConfiguration.Key;
                    paramValidation.ValidAudience = configuration.Audience;
                    paramValidation.ValidIssuer = configuration.Issuer;
                    paramValidation.ValidateIssuerSigningKey = true;
                    paramValidation.ValidateLifetime = true;
                    paramValidation.ClockSkew = TimeSpan.Zero;
                });
            service.AddTransient<IJwtAuthentication, JwtAuthentication>();
            service.AddAuthorization(_ =>
            {
                _.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
                service.AddMvc()
                        .AddNewtonsoftJson();
            });
        }
    }
}
