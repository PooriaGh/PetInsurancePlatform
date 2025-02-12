using System.ComponentModel.DataAnnotations;

namespace PetInsurancePlatform.SharedKernel.Monitoring;

internal sealed class OpenTelemetryOptions
{
    [Required]
    [Url]
    public required Uri Uri { get; set; }
}
