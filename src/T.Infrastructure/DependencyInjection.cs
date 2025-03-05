using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using T.Domain.Interfaces;
using T.Infrastructure.Database;
using T.Infrastructure.Services;

namespace T.Infrastructure {

    public static class DependencyInjection {

        public static IServiceCollection AddHvtContext(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<HvtContext>(options => {
                options.UseNpgsql(configuration.GetConnectionString(nameof(HvtContext)));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }

        public static void AutoMigration(this IServiceProvider serviceProvider) {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HvtContext>();
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
        }

        public static IServiceCollection AddRedis(this IServiceCollection services) {
            services.AddScoped<IRedisService, RedisService>();
            return services;
        }
    }
}
