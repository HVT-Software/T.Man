using System.Reflection;
using FluentValidation;
using T.Application.Base;
using T.Domain.Behaviors;

namespace T.Application {

    public static class DependencyInjection {


        public static IServiceCollection AddApplication(this IServiceCollection services) {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => {
                config.RegisterServicesFromAssemblyContaining<BaseMediatR>();
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            });
            return services;
        }

    }
}
