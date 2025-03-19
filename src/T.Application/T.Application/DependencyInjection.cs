#region

using FluentValidation;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using T.Application.Base;
using T.Application.Base.HttpClients;
using T.Domain.Behaviors;
using T.Domain.Models;

#endregion

namespace T.Application;

public static class DependencyInjection {
    public static void AddApplication(this IServiceCollection services) {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(
            config => {
                config.RegisterServicesFromAssemblyContaining<BaseMediatR>();
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(HeaderMapperBehavior<,>));
            });
    }

    public static void AddAuth(this IServiceCollection services, IConfiguration configuration) {
        Providers? providers = configuration.GetSection("Providers").Get<Providers>();
        if (providers == null) { return; }

        services.AddAuthentication()
            .AddJwtBearer(
                GoogleDefaults.AuthenticationScheme,
                options => {
                    options.Audience  = providers.Google.ClientId;
                    options.Authority = "https://accounts.google.com";
                    options.Challenge = GoogleDefaults.AuthenticationScheme;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer           = true,
                        ValidateAudience         = true,
                        ValidateLifetime         = true,
                        ValidateIssuerSigningKey = true,
                    };
                })
            .AddJwtBearer(
                "GitHub",
                options => {
                    options.Audience  = providers.Github.ClientId;
                    options.Authority = "https://github.com";
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer           = false,
                        ValidateAudience         = false,
                        ValidateLifetime         = false,
                        ValidateIssuerSigningKey = false,
                    };

                    options.Events = new JwtBearerEvents {
                        OnMessageReceived = async context => {
                            string? authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("github ")) {
                                context.Token = authHeader.Substring("github ".Length).Trim();

                                using GitHubTokenClient httpClient = new(providers.Github.ClientId, providers.Github.ClientSecret);
                                if (!string.IsNullOrEmpty(context.Token) && await httpClient.VerifyAccessTokenAsync(context.Token)) {
                                    ClaimsIdentity  identity  = new(null, context.Scheme.Name);
                                    ClaimsPrincipal principal = new(identity);
                                    context.Principal = principal;
                                    context.Success();
                                }
                                else { context.Fail("Invalid GitHub token"); }
                            }
                        },
                    };
                })
            .AddJwtBearer(
                "Discord",
                options => {
                    options.Audience  = providers.Discord.ClientId;
                    options.Authority = "https://discord.com";
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer           = false,
                        ValidateAudience         = false,
                        ValidateLifetime         = false,
                        ValidateIssuerSigningKey = false,
                    };

                    options.Events = new JwtBearerEvents {
                        OnMessageReceived = async context => {
                            string? authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("discord ")) {
                                context.Token = authHeader.Substring("discord ".Length).Trim();

                                using HttpClient httpClient = new();
                                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", context.Token);

                                HttpResponseMessage discordResponse = await httpClient.GetAsync("https://discord.com/api/users/@me");
                                if (discordResponse.IsSuccessStatusCode) {
                                    ClaimsIdentity  identity  = new(null, context.Scheme.Name);
                                    ClaimsPrincipal principal = new(identity);
                                    context.Principal = principal;
                                    context.Success();
                                }
                                else { context.Fail("Invalid Discord token"); }
                            }
                        },
                    };
                })
            .AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                options => options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer           = false,
                    ValidateAudience         = false,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime    = true,
                    IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecret"]!)),
                });

        services.AddAuthorization(
            options => {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(
                        JwtBearerDefaults.AuthenticationScheme,
                        GoogleDefaults.AuthenticationScheme,
                        "GitHub",
                        "Discord").RequireAuthenticatedUser()
                    .Build();
            });

        services.AddAuthorization();
    }
}
