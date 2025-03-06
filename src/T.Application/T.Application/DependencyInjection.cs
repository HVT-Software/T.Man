using T.Application.Base;

namespace T.Application {

    public static class DependencyInjection {


        public static IServiceCollection AddMediatR(this IServiceCollection services) {
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<BaseMediatR>());
            return services;
        }

    }
}
