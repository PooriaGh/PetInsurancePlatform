using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetInsurancePlatform.SharedKernel.Interfaces;

namespace PetInsurancePlatform.Users.Infrastructure.ModuleInstaller;

internal sealed class UsersModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
    }
}
