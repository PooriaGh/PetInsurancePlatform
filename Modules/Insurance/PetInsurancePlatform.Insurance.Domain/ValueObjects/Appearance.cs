using Ardalis.Result;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.ValueObjects;

public sealed class Appearance : ValueObject
{
    // Used by EF Core
    private Appearance()
    {
    }

    public string Value { get; private set; } = string.Empty;

    public static Result<Appearance> Create(string value)
    {
        return new Appearance
        {
            Value = value,
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
