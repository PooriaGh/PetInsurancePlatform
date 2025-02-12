using FastEndpoints;
using FastEndpoints.Swagger;
using PetInsurancePlatform.SharedKernel.Authentication;
using PetInsurancePlatform.SharedKernel.HealthChecks;
using PetInsurancePlatform.SharedKernel.Monitoring;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddFastEndpoints()
    .AddAuthorization()
    .SwaggerDocument();

builder.Services.AddOpenIddictAuthentication();

builder.Services.AddDefaultHealthChecks();

builder.Services.AddOpenTelemetryMonitoring(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseFastEndpoints()
   .UseSwaggerGen();

app.Run();
