using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace T.Api.Extensions;

public static class SwaggerExtension {

    public static IServiceCollection AddSwag(this IServiceCollection services) {
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Doran API v1", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Description = @"API KEY",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
        });
        return services;
    }

    public static IApplicationBuilder UseSwag(this IApplicationBuilder app) {
        app.UseSwagger().UseSwaggerUI(options => {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Doran API v1");
            options.DocExpansion(DocExpansion.None);
        });
        return app;
    }
}
