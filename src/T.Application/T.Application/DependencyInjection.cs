using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using T.Application.Base;
using T.Application.Base.HttpClients;
using T.Domain.Behaviors;
using T.Domain.Models;

namespace T.Application {
    public class DiscordUser {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        // Thêm các thuộc tính khác nếu cần, ví dụ: Discriminator, Email, v.v.
    }
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
                    options.Audience = providers.Google.ClientId;
                    options.Authority = "https://accounts.google.com";
                    options.Challenge = GoogleDefaults.AuthenticationScheme;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                })
                .AddJwtBearer("GitHub", options => {
                    options.Audience = providers.Github.ClientId;
                    options.Authority = "https://github.com";
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                    };

                    options.Events = new JwtBearerEvents {
                        OnMessageReceived = async context => {
                            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("github ")) {
                                context.Token = authHeader.Substring("github ".Length).Trim();

                                using var httpClient = new GitHubTokenClient(providers.Github.ClientId, providers.Github.ClientSecret);
                                if (!string.IsNullOrEmpty(context.Token) && (await httpClient.VerifyAccessTokenAsync(context.Token))) {
                                    var identity = new ClaimsIdentity(null, context.Scheme.Name);
                                    var principal = new ClaimsPrincipal(identity);
                                    context.Principal = principal;
                                    context.Success();
                                } else context.Fail("Invalid GitHub token");
                            }
                        }
                    };
                })
                .AddJwtBearer("Discord", options => {
                    options.Audience = providers.Discord.ClientId;
                    options.Authority = "https://discord.com";
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                    };

                    options.Events = new JwtBearerEvents {
                        OnMessageReceived = async context => {
                            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("discord ")) {
                                context.Token = authHeader.Substring("discord ".Length).Trim();

                                using var httpClient = new HttpClient();
                                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", context.Token);

                                var discordResponse = await httpClient.GetAsync("https://discord.com/api/users/@me");
                                if (discordResponse.IsSuccessStatusCode) {
                                    var identity = new ClaimsIdentity(null, context.Scheme.Name);
                                    var principal = new ClaimsPrincipal(identity);
                                    context.Principal = principal;
                                    context.Success();
                                } else context.Fail("Invalid Discord token");
                            }
                        }
                    };
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecret"]!))
                });

            services.AddAuthorization(options => {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme, GoogleDefaults.AuthenticationScheme, "GitHub", "Discord")
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddAuthorization();
            return services;
        }

    }
}
