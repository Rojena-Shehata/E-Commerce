using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_Commerce.Web.Extensions
{
    public static class AddServices
    {
        public static IServiceCollection AddAuthenticationService(this IServiceCollection authService, IConfiguration configuration)
        {

            var jwtOptions = configuration.GetSection("JWTOptions").Get<JWTOptionsDTO>();
            authService.AddAuthentication(configureOptions =>
            {
                configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//"Bearer"
                configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JWTBearerOptions =>
            {
                JWTBearerOptions.SaveToken = true;
                JWTBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey)),

                    ValidateLifetime = true,
                };
            });

            return authService;
        }
    }
}
