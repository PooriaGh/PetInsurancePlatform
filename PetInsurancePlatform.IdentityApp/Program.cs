using PetInsurancePlatform.IdentityApp.Authentication;
using PetInsurancePlatform.IdentityApp.Cors;
using PetInsurancePlatform.IdentityApp.Data;
using PetInsurancePlatform.IdentityApp.Identity;
using PetInsurancePlatform.IdentityApp.Queue;
using PetInsurancePlatform.SharedKernel.HealthChecks;
using PetInsurancePlatform.SharedKernel.Monitoring;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddIdentityProvider();

builder.Services.AddQueue();

builder.Services.AddDefaultCorsPolicy();

builder.Services.AddGoogleAuthentication();

builder.Services.AddDefaultHealthChecks();

builder.Services.AddOpenTelemetryMonitoring(Assembly.GetExecutingAssembly());

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
