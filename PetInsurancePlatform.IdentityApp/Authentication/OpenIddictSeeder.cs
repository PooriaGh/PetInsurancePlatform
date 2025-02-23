using OpenIddict.Abstractions;
using PetInsurancePlatform.IdentityApp.Data;

namespace PetInsurancePlatform.IdentityApp.Authentication;

public sealed class OpenIddictSeeder(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        if (!env.IsDevelopment())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.EnsureCreatedAsync(cancellationToken);

            var applicationManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            await applicationManager.RegisterAsync();


            var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
            await scopeManager.RegisterAsync();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
