using T.Api.Extensions;
using T.Application;
using T.Domain;
using T.Domain.Middlewares;
using T.Infrastructure;

namespace T.Api {

    public class Program {

        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHvtContext(builder.Configuration);

            builder.Services.AddAuth(builder.Configuration);
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwag();
            builder.Services.AddCors();

            builder.Services.AddMiddlewares();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddExceptionHandler<ExceptionHandler>();

            builder.Services.AddApplication();
            builder.Services.AddRedis(builder.Configuration);

            var app = builder.Build();


            app.UseSwag();
            app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddlewares();
            app.MapControllers();
            app.Services.AutoMigration();
            app.Run();
        }
    }
}
