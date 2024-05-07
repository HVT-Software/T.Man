using DR.Api.Extentions;
using DR.Application;
using DR.Domain;
using DR.Domain.Extentions;
using DR.Infrastructure;

namespace DR.Api {

    public class Program {

        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDoranContext(builder.Configuration);

            builder.Services.AddJWT(builder.Configuration);
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors();
            builder.Services.AddMiddlewares();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddMediatR();
            builder.Services.AddRedis(builder.Configuration);

            var app = builder.Build();

            app.UseSwag();

            app.UseHttpsRedirection();
            app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));
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
