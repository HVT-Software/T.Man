#region

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using T.Domain.Common.Configs;
using T.Domain.Interfaces;
using T.Infrastructure.Database;
using T.Infrastructure.Services;

#endregion

namespace T.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddHvtContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HvtContext>(
            options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(HvtContext)));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

        return services;
    }

    public static void AutoMigration(this IServiceProvider serviceProvider)
    {
        using IServiceScope? scope     = serviceProvider.CreateScope();
        HvtContext?          dbContext = scope.ServiceProvider.GetRequiredService<HvtContext>();
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        RedisConfig? config = configuration.GetSection("Redis").Get<RedisConfig>();
        services.AddSingleton<IConnectionMultiplexer>(
            _ => ConnectionMultiplexer.Connect($"{config?.Host}:{config?.Port},password={config?.Password}"));
        services.AddScoped<IRedisService, RedisService>();
        return services;
    }
}
