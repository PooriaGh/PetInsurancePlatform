using FastEndpoints;
using FastEndpoints.Swagger;
using PetInsurancePlatform.Insurance.Infrastructure.ModuleInstaller;
using PetInsurancePlatform.SharedKernel.Authentication;
using PetInsurancePlatform.SharedKernel.Extensions;
using PetInsurancePlatform.SharedKernel.HealthChecks;
using PetInsurancePlatform.SharedKernel.Monitoring;
using PetInsurancePlatform.Users.Infrastructure.ModuleInstaller;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddFastEndpoints()
    .AddAuthorization()
    .SwaggerDocument();

builder.Services.AddOpenIddictAuthentication();

builder.Services.AddDefaultHealthChecks();

builder.Services.AddOpenTelemetryMonitoring(Assembly.GetExecutingAssembly());

builder.Services.InstallModules(
    builder.Configuration,
    InsuranceModule.Assembly,
    UsersModule.Assembly);

var app = builder.Build();

app.UseFastEndpoints()
   .UseSwaggerGen();

app.Run();
