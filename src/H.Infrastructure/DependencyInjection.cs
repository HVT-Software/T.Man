using H.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace H.Infrastructure {

    public static class DependencyInjection {

        public static IServiceCollection AddDoranContext(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<DrContext>(options => {
                options.UseNpgsql(configuration.GetConnectionString(nameof(DrContext)));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }

        public static void AutoMigration(this IServiceProvider serviceProvider) {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DrContext>();
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
        }
    }
}
