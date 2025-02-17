using Ardalis.Result;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.ValueObjects;

public sealed class AgeRange : ValueObject
{
    public static readonly AgeRange None = new();

    // Used by EF Core
    private AgeRange()
    {
    }

    public int MaximumValue { get; private set; }

    public int MinimumValue { get; private set; }

    public static Result<AgeRange> Create(
        int maximumValue,
        int minimumValue)
    {
        if (maximumValue == 0)
        {
            return Result.Invalid(new ValidationError("The maximum value is required."));
        }

        if (minimumValue == 0)
        {
            return Result.Invalid(new ValidationError("The minimum value is required."));
        }

        if (maximumValue < minimumValue)
        {
            return Result.Invalid(new ValidationError("The maximum value must be equal or greater than the minimum value."));
        }

        return new AgeRange
        {
            MaximumValue = maximumValue,
            MinimumValue = minimumValue,
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MaximumValue;
        yield return MinimumValue;
    }
}
