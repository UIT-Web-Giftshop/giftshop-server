using System.Text;
using Infrastructure.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.ServicesExtension
{
    public static class AuthenticationServiceExtension
    {
        public static IServiceCollection AddAuthenticationService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var authenticationSettings = configuration.GetSection("AuthenticationSettings");
            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(authenticationSettings.GetSection("SecretKey").Value));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAccessor, Accessor>();

            return services;
        }
    }
}