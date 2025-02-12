using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Animals.Domain.Diseases;

public sealed class Disease(Guid id) : Entity(id)
{
    public string Name { get; private set; } = null!;

    public bool Accepted { get; private set; }

    public static Disease Create(
        string name,
        bool accepted)
    {
        return new Disease(Guid.NewGuid())
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
