using T.Api.Common.Base;
using T.Api.Middlewares;

namespace T.Api {

    public static class DependencyInjection {

        public static IServiceCollection AddMediatR(this IServiceCollection services) {
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<BaseMediatR>());
            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) {
            //var firebaseConfig = configuration.GetSection("Firebase").Get<FirebaseConfig>();
            //FirebaseApp.Create(new AppOptions() {
            //    Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(firebaseConfig))
            //});
            return services;
        }

        public static IServiceCollection AddMiddlewares(this IServiceCollection services) {
            services.AddScoped<ExceptionMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app) {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
}
