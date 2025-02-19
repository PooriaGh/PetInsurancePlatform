using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PetInsurancePlatform.SharedKernel.Interfaces;

public interface IModuleInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
