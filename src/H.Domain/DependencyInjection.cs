using H.Domain.ExternalServices.Implements;
using H.Domain.ExternalServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace H.Domain {

    public static class DependencyInjection {

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration) {
            services.AddScoped<IRedisService, RedisService>();
            return services;
        }
    }
}
