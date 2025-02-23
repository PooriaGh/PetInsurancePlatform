using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class PetTypeDisease : Entity
{
    // Used by EF Core
    private PetTypeDisease()
    {
    }
    public DateTime CreatedAt { get; private set; }

    public PetType PetType { get; private set; } = PetType.None;

    public Disease Disease { get; private set; } = Disease.None;

    internal static PetTypeDisease Create(
        PetType petType,
        Disease disease)
    {
        return new PetTypeDisease
        {
            CreatedAt = DateTime.UtcNow,
            PetType = petType,
            Disease = disease,
        };
    }
}
