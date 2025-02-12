using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PetInsurancePlatform.SharedKernel.Monitoring;

internal sealed class OpenTelemetryOptionsSetup(IConfiguration configuration) : IConfigureOptions<OpenTelemetryOptions>
{
    public const string SECTION_NAME = "OpenTelemetry";

    public void Configure(OpenTelemetryOptions options)
    {
        configuration.GetSection(SECTION_NAME).Bind(options);
    }
}
