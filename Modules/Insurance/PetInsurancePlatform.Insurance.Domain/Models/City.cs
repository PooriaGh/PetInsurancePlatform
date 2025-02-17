using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class City : Entity
{
    public static readonly City None = new();

    // Used by EF Core
    private City() : base()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public Province Province { get; set; } = Province.None;

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
