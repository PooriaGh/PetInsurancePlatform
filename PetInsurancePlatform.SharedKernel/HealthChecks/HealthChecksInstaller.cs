using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PetInsurancePlatform.SharedKernel.HealthChecks;

public static class HealthChecksInstaller
{
    public static void AddDefaultHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());
    }
}
