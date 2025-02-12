using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Animals.Domain.AnimalTypes;

public sealed class Address(
    string district,
    string street,
    string? alley = null,
    string? plateNumber = null,
    string? postalCode = null) : ValueObject
{
    public string District { get; private set; } = district;

    public string Street { get; private set; } = street;

    public string? Alley { get; private set; } = alley;

    public string? PlateNumber { get; private set; } = plateNumber;

    public string? PostalCode { get; private set; } = postalCode;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return District;
        yield return Street;
        yield return Alley ?? string.Empty;
        yield return PlateNumber ?? string.Empty;
        yield return PostalCode ?? string.Empty;
    }
}
