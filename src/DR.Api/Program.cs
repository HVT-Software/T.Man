using System.Text;
using DR.Application;
using DR.Domain;
using DR.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DR.Api {

    public class Program {

        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDoranContext(builder.Configuration);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecret"]!))
                });

            _ = builder.Services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());

            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Doran API v1", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = @"API KEY",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddCors();
            builder.Services.AddMiddlewares();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddMediatR();
            builder.Services.AddRedis(builder.Configuration);

            var app = builder.Build();

            app.UseSwagger().UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FMS Notification API v1"));
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
