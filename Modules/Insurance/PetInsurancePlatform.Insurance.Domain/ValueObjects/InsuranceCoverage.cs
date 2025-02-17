using Ardalis.Result;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.ValueObjects;

public sealed class InsuranceCoverage : ValueObject
{
    // Used by EF Core
    private InsuranceCoverage()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public static Result<InsuranceCoverage> Create(string name)
    {
        return new InsuranceCoverage
        {
            Name = name,
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}
