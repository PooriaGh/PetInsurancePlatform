using Ardalis.Result;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    public static readonly Address None = new();

    // Used by EF Core
    private Address()
    {
    }

    public int District { get; private set; }

    public string Street { get; private set; } = string.Empty;

    public string? Alley { get; private set; }

    public string? PlateNumber { get; private set; }

    public string? PostalCode { get; private set; }

    public static Result<Address> Create(
        int district,
        string street,
        string? alley = null,
        string? plateNumber = null,
        string? postalCode = null)
    {
        if (district == 0)
        {
            return Result.Invalid(new ValidationError("The district of address is required."));
        }

        if (district < 0)
        {
            return Result.Invalid(new ValidationError("The district of address must be a non-negative value."));
        }

        if (string.IsNullOrWhiteSpace(street))
        {
            return Result.Invalid(new ValidationError("The street of address is required."));
        }

        return new Address
        {
            District = district,
            Street = street,
            Alley = alley,
            PlateNumber = plateNumber,
            PostalCode = postalCode,
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return District;
        yield return Street;
        yield return Alley ?? string.Empty;
        yield return PlateNumber ?? string.Empty;
        yield return PostalCode ?? string.Empty;
    }
}
