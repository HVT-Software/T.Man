using H.Api.Extentions;
using H.Domain;
using H.Infrastructure;

namespace H.Api {

    public class Program {

        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDoranContext(builder.Configuration);
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwag();
            builder.Services.AddJWT(builder.Configuration);
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

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseMiddlewares();
            app.MapControllers();
            app.Services.AutoMigration();
            app.Run();
        }
    }
}
