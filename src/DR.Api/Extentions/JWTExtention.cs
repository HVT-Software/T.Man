using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace DR.Api.Extentions;

public static class JWTExtention {

    public static IServiceCollection AddJWT(this IServiceCollection services, IConfiguration configuration) {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters {
                  ValidateIssuer = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  RequireExpirationTime = false,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecret"]!))
              });

        services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser().Build());

        return services;
    }
}
