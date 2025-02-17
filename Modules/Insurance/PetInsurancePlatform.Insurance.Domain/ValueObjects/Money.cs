using Ardalis.Result;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    public static readonly Money Zero = new();

    // Used by EF Core
    private Money()
    {
    }

    public long Value { get; private set; }

    public static Result<Money> Create(int value)
    {
        if (value == 0)
        {
            return Result.Invalid(new ValidationError("The value of money is required."));
        }

        if (value < 0)
        {
            return Result.Invalid(new ValidationError("The value of money must be a non-negative value."));
        }

        return new Money
        {
            Value = value,
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
