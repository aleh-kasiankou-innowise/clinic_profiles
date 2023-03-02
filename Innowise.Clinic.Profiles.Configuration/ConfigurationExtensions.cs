using System.Text;
using Innowise.Clinic.Profiles.Configuration.Swagger.Examples;
using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.Services.ConsistencyManager.Implementations;
using Innowise.Clinic.Profiles.Services.ConsistencyManager.Interfaces;
using Innowise.Clinic.Profiles.Services.DoctorService.Implementations;
using Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;
using Innowise.Clinic.Profiles.Services.PatientService.Implementations;
using Innowise.Clinic.Profiles.Services.PatientService.Interfaces;
using Innowise.Clinic.Profiles.Services.ProfileLinkingService.Implementations;
using Innowise.Clinic.Profiles.Services.ProfileLinkingService.Interfaces;
using Innowise.Clinic.Profiles.Services.RabbitMq.Options;
using Innowise.Clinic.Profiles.Services.RabbitMq.RabbitMqConsumer;
using Innowise.Clinic.Profiles.Services.RabbitMq.RabbitMqPublisher;
using Innowise.Clinic.Profiles.Services.ReceptionistService.Implementations;
using Innowise.Clinic.Profiles.Services.ReceptionistService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Innowise.Clinic.Profiles.Configuration;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opts =>
        {
            opts.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                    },
                    new string[] { }
                }
            });
            opts.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblyOf<CreatePatientProfileExamples>();
        services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
        return services;
    }

    public static IServiceCollection ConfigureCrossServiceCommunication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitOptions>(configuration.GetSection("RabbitConfigurations"));
        services.AddScoped<IConsistencyService, ConsistencyService>();
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        services.AddHostedService<RabbitMqConsumer>();
        return services;
    }

    public static IServiceCollection ConfigureProfileServices(this IServiceCollection services)
    {
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IReceptionistService, ReceptionistService>();
        services.AddScoped<IProfileLinkingService, ProfileLinkingService>();
        return services;
    }

    public static IServiceCollection ConfigureSecurity(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    Environment.GetEnvironmentVariable("JWT__KEY") ?? throw new
                        InvalidOperationException()))
            };
        });
        return services;
    }

    public static async Task PrepareDb(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ProfilesDbContext>();
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }
    }
}