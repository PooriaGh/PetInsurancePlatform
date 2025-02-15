using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class Disease : Entity
{
    // Used by EF Core
    private Disease() : base()
    {
    }

    public string Name { get; private set; } = null!;
    public bool Accepted { get; private set; }

    private readonly List<PetTypeDisease> _animalTypeDiseases = [];
    public IReadOnlyCollection<PetType> AnimalTypes => _animalTypeDiseases
        .OrderBy(atd => atd.CreatedAt)
        .Select(d => d.Type)
        .ToList();

    public static Disease Create(
        string name,
        bool accepted)
    {
        return new Disease
        {
            Name = name,
            Accepted = accepted,
        };
    }

    public void Update(
        string name,
        bool accepted)
    {
        Name = name;
        Accepted = accepted;
    }
}
