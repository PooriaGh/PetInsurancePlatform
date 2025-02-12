using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Animals.Domain.AnimalTypes;

public sealed class AppearanceCharacteristic(string value) : ValueObject
{
    public string Value { get; private set; } = value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
