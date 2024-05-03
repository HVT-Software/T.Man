using DR.Domain.Services.Implements;
using DR.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DR.Domain {

    public static class DependencyInjection {

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration) {
            services.AddScoped<IRedisService, RedisService>();
            return services;
        }
    }
}
