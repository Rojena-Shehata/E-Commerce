using E_Commerce.Shared.DTOs.IdentityDTOs;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
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

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
            });

            return services;
        }
       
        public static IServiceCollection ConfigureApiBehaviourOptions(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;

            });
            return services;
        }


        public static IServiceCollection AddCORsPolicy(this IServiceCollection services)
        {
            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("AllowAll", corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyHeader();
                    corsPolicyBuilder.AllowAnyMethod();
                    corsPolicyBuilder.AllowAnyOrigin();
                });
            });

            return services;
        }

    }
}
