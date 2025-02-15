namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class PetTypeDisease
{
    // Used by EF Core
    private PetTypeDisease()
    {
    }

    public PetType Type { get; private set; } = null!;

    public Disease Disease { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    internal static PetTypeDisease Create(
        PetType type,
        Disease disease)
    {
        return new PetTypeDisease
        {
            Type = type,
            Disease = disease,
            CreatedAt = DateTime.UtcNow,
        };
    }
}
