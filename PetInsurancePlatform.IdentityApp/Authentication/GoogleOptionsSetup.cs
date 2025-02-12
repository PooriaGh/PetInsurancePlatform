using Microsoft.Extensions.Options;

namespace PetInsurancePlatform.IdentityApp.Authentication;

internal sealed class GoogleOptionsSetup(IConfiguration configuration) : IConfigureOptions<GoogleOptions>
{
    public const string SECTION_NAME = "Google";

    public void Configure(GoogleOptions options)
    {
        configuration.GetSection(SECTION_NAME).Bind(options);
    }
}
