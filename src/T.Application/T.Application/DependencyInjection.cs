using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using T.Application.Base;
using T.Domain.Behaviors;
using T.Domain.Models;

namespace T.Application {

    public static class DependencyInjection {


        public static IServiceCollection AddApplication(this IServiceCollection services) {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => {
                config.RegisterServicesFromAssemblyContaining<BaseMediatR>();
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            });
            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
            var providers = configuration.GetSection("Providers").Get<Providers>();
            if (providers == null) return services;

            services.AddAuthentication()
                .AddJwtBearer(GoogleDefaults.AuthenticationScheme, options => {
                    options.Audience = configuration["Authentication:Google:ClientId"];
                    options.Authority = "https://accounts.google.com";
                    options.Challenge = GoogleDefaults.AuthenticationScheme;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecret"]!))
                });

            services.AddAuthorization(options => {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme, GoogleDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddAuthorization();
            return services;
        }

    }
}
