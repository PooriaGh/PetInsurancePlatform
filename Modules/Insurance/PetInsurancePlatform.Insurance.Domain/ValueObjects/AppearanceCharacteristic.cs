using Ardalis.Result;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.ValueObjects;

public sealed class AppearanceCharacteristic : ValueObject
{
    // Used by EF Core
    private AppearanceCharacteristic()
    {
    }

    public string Value { get; private set; } = string.Empty;

    public static Result<AppearanceCharacteristic> Create(string value)
    {
        return new AppearanceCharacteristic
        {
            Value = value,
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
