#region

using T.Api.Extensions;
using T.Application;
using T.Domain;
using T.Domain.Common.Configs;
using T.Infrastructure;

#endregion

namespace T.Api;

public class Program {
    public static void Main(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection(nameof(EmailConfig)));

        builder.Services.AddHvtContext(builder.Configuration);
        builder.Services.AddAuth(builder.Configuration);
        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwag();
        builder.Services.AddCors();

        builder.Services.AddMiddlewares();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddApplication();
        builder.Services.AddRedis(builder.Configuration);
        builder.Services.AddDomainServices();

        WebApplication app = builder.Build();

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
