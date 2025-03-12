using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
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

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = providers.Jwt.Issuer,
                        ValidAudience = providers.Jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(providers.Jwt.Key))
                    };
                })
                .AddGoogle("Google", options => {
                    options.ClientId = providers.Google.ClientId;
                    options.ClientSecret = providers.Google.ClientSecret;
                    options.CallbackPath = "/api/auth/callback/Google";
                })
                .AddOAuth("Github", options => {
                    options.ClientId = providers.Github.ClientId;
                    options.ClientSecret = providers.Github.ClientSecret;
                    options.CallbackPath = "/api/auth/callback/github";
                    options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                    options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                    options.UserInformationEndpoint = "https://api.github.com/user";
                    options.SaveTokens = true;
                    options.Events = new OAuthEvents {
                        OnCreatingTicket = async context => {
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                            var response = await context.Backchannel.SendAsync(request);
                            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                            context.RunClaimActions(user.RootElement);
                        }
                    };
                })
                .AddOAuth("Discord", options => {
                    options.ClientId = providers.Discord.ClientId;
                    options.ClientSecret = providers.Discord.ClientSecret;
                    options.CallbackPath = "/api/auth/callback/Discord";
                    options.AuthorizationEndpoint = "https://discord.com/api/oauth2/authorize";
                    options.TokenEndpoint = "https://discord.com/api/oauth2/token";
                    options.UserInformationEndpoint = "https://discord.com/api/users/@me";
                    options.Scope.Add("identify");
                    options.SaveTokens = true;
                    options.Events = new OAuthEvents {
                        OnCreatingTicket = async context => {
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                            var response = await context.Backchannel.SendAsync(request);
                            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                            context.RunClaimActions(user.RootElement);
                        }
                    };
                });

            services.AddAuthorization();
            return services;
        }

    }
}
