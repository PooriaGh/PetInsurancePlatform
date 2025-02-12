using PetInsurancePlatform.Animals.Domain.Diseases;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Animals.Domain.AnimalTypes;

public sealed class AnimalTypeDisease(Guid id) : Entity(id)
{
    public Disease Disease { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    internal static AnimalTypeDisease Create(Disease disease)
    {
        return new AnimalTypeDisease(Guid.NewGuid())
        {
            Disease = disease,
            CreatedAt = DateTime.UtcNow,
        };
    }
}
