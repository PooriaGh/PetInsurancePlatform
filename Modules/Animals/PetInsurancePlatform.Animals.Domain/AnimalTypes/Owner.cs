using PetInsurancePlatform.Animals.Domain.Provinces;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Animals.Domain.AnimalTypes;

public sealed class Owner(Guid id) : Entity(id)
{
    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public string FullName => FirstName + " " + LastName;

    public string NationalID { get; private set; } = null!;

    public DateOnly DateOfBirth { get; private set; }

    public City City { get; private set; } = null!;

    public IReadOnlyCollection<Animal> Animals { get; private set; } = [];
}
