using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace T.Domain {

    public static class DependencyInjection {
        public static IServiceCollection AddMiddlewares(this IServiceCollection services) {
            return services;
        }

        public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app) {
            return app;
        }
    }
}
