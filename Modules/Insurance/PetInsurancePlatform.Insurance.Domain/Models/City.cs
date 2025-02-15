using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class City : Entity
{
    // Used by EF Core
    private City() : base()
    {
    }

    public string Name { get; private set; } = null!;
    public Province Province { get; set; } = null!;

    public static City Create(string name)
    {
        return new City
        {
            Name = name,
        };
    }

    public void Update(string name)
    {
        Name = name;
    }
}
