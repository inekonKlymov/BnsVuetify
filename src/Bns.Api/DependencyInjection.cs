//using Bns.Api.Auth;
using Bns.Domain.Common.Errors;
using Bns.Domain.Users;
using Bns.Infrastructure.Database;
using FluentResults.Extensions.AspNetCore;
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


            //services.AddScoped<IJwtUtils, JwtUtils>();
            //services.AddScoped<IUserService, UserService>();
            services.AddSecutiry(configuration);
            // Register API-specific services here
            // Example: services.AddControllers();

            // Register other layers' dependencies

            return services;
        }
        private static IServiceCollection AddSecutiry(this IServiceCollection services, IConfiguration configuration)
        {
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
            services.AddAuthorization();
            services.AddAuthentication(IdentityConstants.BearerScheme)
                //.AddCookie(IdentityConstants.ApplicationScheme)
                .AddBearerToken(IdentityConstants.BearerScheme, opts =>
                {
                    //opts.RefreshTokenExpiration = TimeSpan.FromHours(2);
                    opts.BearerTokenExpiration = TimeSpan.FromHours(1);

                });
            services.AddIdentityCore<User>(options=>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                
                //options.User.
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();

            //var jwtKey = configuration["Jwt:Key"];
            //var jwtIssuer = configuration["Jwt:Issuer"];
            //var jwtAudience = configuration["Jwt:Audience"];
            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = jwtIssuer, // ваш издатель
            //            ValidAudience = jwtAudience, // ваша аудитория
            //            IssuerSigningKey = new SymmetricSecurityKey(
            //                Encoding.UTF8.GetBytes(jwtKey)) // секретный ключ
            //        };
            //    });
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