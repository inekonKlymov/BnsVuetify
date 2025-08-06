//using Bns.Api.Auth;
using Bns.Domain.Common.Errors;
using Bns.Domain.Users;
using Bns.Infrastructure.Database;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Bns.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            AspNetCoreResult.Setup(config => config.DefaultProfile = new ErrorsAspMapProfile());

            //services.AddIdentitContextJwtSecutiry(configuration);
            services.AddKeyCloakSecutiry(configuration);

            return services;
        }

        private static IServiceCollection AddKeyCloakSecutiry(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["Keycloak:Authority"];
                    options.Audience = configuration["Keycloak:Audience"];
                    options.RequireHttpsMetadata = false; // true для production с HTTPS
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Используем Authority для ValidIssuer
                        ValidIssuer = configuration["Keycloak:Authority"],
                        ValidateIssuer = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception}");
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddCors(options =>
            {
                options.AddPolicy(Authorization.Policies.VuePolicy, policy =>
                {
                    policy
                    .WithOrigins("http://localhost:5173", "https://localhost:5173") //vue
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                    //.AllowAnyOrigin()
                    ;
                });
            });
            return services;
        }

    }

    public static class Authorization
    {
        public static class Policies
        {
            public const string VuePolicy = "AllowVueApp";
        }
    }
}