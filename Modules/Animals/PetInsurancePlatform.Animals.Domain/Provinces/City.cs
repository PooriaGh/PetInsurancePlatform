using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Animals.Domain.Provinces;

public sealed class City(Guid id) : Entity(id)
{
    public string Name { get; private set; } = null!;
    public Province Province { get; set; } = null!;

    public static City Create(string name)
    {
        return new City(Guid.NewGuid())
        {
            Name = name,
        };
    }

    public void Update(string name)
    {
        Name = name;
    }
}
