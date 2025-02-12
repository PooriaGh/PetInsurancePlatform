using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PetInsurancePlatform.SharedKernel.Authentication;

internal sealed class OpenIddictOptionsSetup(IConfiguration configuration) : IConfigureOptions<OpenIddictOptions>
{
    public const string SECTION_NAME = "OpenIddict";

    public void Configure(OpenIddictOptions options)
    {
        configuration.GetSection(SECTION_NAME).Bind(options);
    }
}
