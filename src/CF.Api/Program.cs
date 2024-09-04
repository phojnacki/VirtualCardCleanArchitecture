using System.IO.Compression;
using CF.Api.Filters;
using CF.Api.Middleware;
using CF.VirtualCard.Application.Facades;
using CF.VirtualCard.Application.Facades.Interfaces;
using CF.VirtualCard.Domain.Repositories;
using CF.VirtualCard.Domain.Services;
using CF.VirtualCard.Domain.Services.Interfaces;
using CF.VirtualCard.Infrastructure.DbContext;
using CF.VirtualCard.Infrastructure.Mappers;
using CF.VirtualCard.Infrastructure.Repositories;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.SwaggerGen;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

builder.Services.AddControllers(x => x.Filters.Add<ExceptionFilter>());
builder.Services.AddProblemDetails();
builder.Services.AddDefaultCorrelationId(ConfigureCorrelationId());
builder.Services.AddTransient<IVirtualCardFacade, VirtualCardFacade>();
builder.Services.AddTransient<IVirtualCardService, VirtualCardService>();
builder.Services.AddTransient<IVirtualCardRepository, VirtualCardRepository>();
builder.Services.AddTransient<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddAutoMapper(typeof(VirtualCardProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SetupSwagger());
builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
builder.Services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
builder.Services.AddDbContext<VirtualCardContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"),
        a => { a.MigrationsAssembly("CF.Migrations"); });
});

AddNLog();

await using var app = builder.Build();

RunMigration();
app.UseCorrelationId();
AddExceptionHandler();
AddSwagger();
app.UseMiddleware<LogExceptionMiddleware>();
app.UseMiddleware<LogRequestMiddleware>();
app.UseMiddleware<LogResponseMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();

void AddExceptionHandler()
{
    if (app.Environment.IsDevelopment()) return;
    app.UseExceptionHandler(ConfigureExceptionHandler());
}

void AddSwagger()
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CF Api"));
}

void AddNLog()
{
    if (builder.Environment.EnvironmentName.Contains("Test")) return;
    LogManager.Setup().LoadConfigurationFromSection(builder.Configuration);
}

Action<SwaggerGenOptions> SetupSwagger()
{
    return c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "CF API", Version = "v1" });

        c.CustomSchemaIds(x => x.FullName);

        c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            Description = "JWT Authorization header using the Bearer scheme",
            In = ParameterLocation.Header
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    }
                },
                new List<string>()
            }
        });
    };
}

Action<CorrelationIdOptions> ConfigureCorrelationId()
{
    return options =>
    {
        options.LogLevelOptions = new CorrelationIdLogLevelOptions
        {
            FoundCorrelationIdHeader = LogLevel.Debug,
            MissingCorrelationIdHeader = LogLevel.Debug
        };
    };
}

Action<IApplicationBuilder> ConfigureExceptionHandler()
{
    return exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                Message = "An unexpected internal exception occurred."
            });
        });
    };
}

void RunMigration()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    if (!serviceScope.ServiceProvider.GetRequiredService<VirtualCardContext>().Database.GetPendingMigrations()
            .Any()) return;

    serviceScope.ServiceProvider.GetRequiredService<VirtualCardContext>().Database.Migrate();
}

public partial class Program
{
}