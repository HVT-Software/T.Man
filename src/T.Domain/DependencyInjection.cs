using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using T.Domain.ExternalServices.Implements;
using T.Domain.ExternalServices.Interfaces;

namespace T.Domain {

    public static class DependencyInjection {

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration) {
            services.AddScoped<IRedisService, RedisService>();
            return services;
        }
    }
}
