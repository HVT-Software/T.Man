#region

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using T.Domain.Interfaces;
using T.Domain.Middlewares;
using T.Domain.Services;

#endregion

namespace T.Domain;

public static class DependencyInjection {
    public static IServiceCollection AddMiddlewares(this IServiceCollection services) {
        services.AddScoped<ExceptionMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app) {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }

    public static void AddTranslateService(this IServiceCollection services) {
        services.AddHttpClient(
            "datpmt",
            c => {
                c.BaseAddress = new Uri("https://api.datpmt.com/api/v2/dictionary/");
                c.Timeout     = TimeSpan.FromSeconds(10);
            });
        services.AddScoped<ITranslateService, TranslateService>();
    }
}
