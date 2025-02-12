using System.ComponentModel.DataAnnotations;

namespace PetInsurancePlatform.IdentityApp.Authentication;

internal sealed class GoogleOptions
{
    [Required]
    public required string ClientId { get; set; }

    [Required]
    public required string ClientSecret { get; set; }
}
