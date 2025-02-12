using System.ComponentModel.DataAnnotations;

namespace PetInsurancePlatform.IdentityApp.Queue;

internal sealed class QueueOptions
{
    [Required]
    public required string Host { get; set; }

    [Required]
    public required string UserName { get; set; }

    [Required]
    public required string Password { get; set; }
}
