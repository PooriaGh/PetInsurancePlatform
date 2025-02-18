using Ardalis.Result;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    public static readonly Address None = new();

    public static readonly ValidationError EmptyDistrict = new("The district of address is required.");

    public static readonly ValidationError InvalidDistrict = new("The district of address must have a positive value.");

    public static readonly ValidationError EmptyStreet = new("The street of address is required.");

    public static readonly ValidationError InvalidPostalCode = new("The postal code of address must have a positive value.");

    // Used by EF Core
    private Address()
    {
    }

    public int District { get; private set; }

    public string Street { get; private set; } = string.Empty;

    public string? Alley { get; private set; }

    public string? PlateNumber { get; private set; }

    public long PostalCode { get; private set; }

    public static Result<Address> Create(
        int district,
        string street,
        string? alley,
        string? plateNumber,
        long postalCode)
    {
        if (district == 0)
        {
            return Result.Invalid(EmptyDistrict);
        }

        if (district < 0)
        {
            return Result.Invalid(InvalidDistrict);
        }

        if (string.IsNullOrWhiteSpace(street))
        {
            return Result.Invalid(EmptyStreet);
        }

        if (district < 0)
        {
            return Result.Invalid(InvalidPostalCode);
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
        yield return PostalCode;
    }
}
