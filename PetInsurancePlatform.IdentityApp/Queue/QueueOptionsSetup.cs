using Microsoft.Extensions.Options;

namespace PetInsurancePlatform.IdentityApp.Queue;

internal sealed class QueueOptionsSetup(IConfiguration configuration) : IConfigureOptions<QueueOptions>
{
    public const string SECTION_NAME = "Queue";

    public void Configure(QueueOptions options)
    {
        configuration.GetSection(SECTION_NAME).Bind(options);
    }
}
