using FastEndpoints;
using FastEndpoints.Swagger;
using PetInsurancePlatform.Insurance.Endpoints;
using PetInsurancePlatform.Insurance.Infrastructure.Extensions;
using PetInsurancePlatform.Insurance.Infrastructure.ModuleInstaller;
using PetInsurancePlatform.SharedKernel.Authentication;
using PetInsurancePlatform.SharedKernel.Extensions;
using PetInsurancePlatform.SharedKernel.HealthChecks;
using PetInsurancePlatform.SharedKernel.Monitoring;
using PetInsurancePlatform.Users.Infrastructure.ModuleInstaller;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddFastEndpoints(options =>
    {
        options.SourceGeneratorDiscoveredTypes.AddRange(InsuranceEndpoints.Assembly.ExportedTypes);
    })
    .AddAuthorization()
    .SwaggerDocument();

builder.Services.AddOpenIddictAuthentication();

builder.Services.AddDefaultHealthChecks();

if (builder.Environment.IsProduction())
{
    builder.Services.AddOpenTelemetryMonitoring(Assembly.GetExecutingAssembly());
}

builder.Services.InstallModules(
    builder.Configuration,
    InsuranceModule.Assembly,
    UsersModule.Assembly);

var app = builder.Build();

app
    .UseFastEndpoints(config =>
    {
        config.Binding.UsePropertyNamingPolicy = true;
        config.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;

        config.Endpoints.RoutePrefix = "api";
    })
   .UseSwaggerGen();

if (app.Environment.IsDevelopment())
{
    app.ApplyInsuranceDatabaseMigrations();
}

app.Run();

public partial class Program
{
}
