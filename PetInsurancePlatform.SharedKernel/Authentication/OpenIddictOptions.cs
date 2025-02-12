using System.ComponentModel.DataAnnotations;

namespace PetInsurancePlatform.SharedKernel.Authentication;

internal sealed class OpenIddictOptions
{
    [Required]
    public required string Issuer { get; set; }

    [Required]
    public required string ClientId { get; set; }

    [Required]
    public required string ClientSecret { get; set; }
}
