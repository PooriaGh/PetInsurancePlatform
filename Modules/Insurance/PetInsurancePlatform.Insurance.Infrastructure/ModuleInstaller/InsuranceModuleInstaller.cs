using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetInsurancePlatform.Insurance.Infrastructure.Data;
using PetInsurancePlatform.SharedKernel.Interfaces;

namespace PetInsurancePlatform.Insurance.Infrastructure.ModuleInstaller;

internal sealed class InsuranceModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
    }
}
