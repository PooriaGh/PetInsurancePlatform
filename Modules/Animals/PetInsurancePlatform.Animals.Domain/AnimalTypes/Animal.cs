using PetInsurancePlatform.Animals.Domain.Provinces;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Animals.Domain.AnimalTypes;

public sealed class Animal(Guid id) : Entity(id)
{
    public string Name { get; private set; } = null!;

    public string Breed { get; private set; } = null!;

    public Gender Gender { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public long Price { get; private set; }

    public City City { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    public IReadOnlyCollection<AppearanceCharacteristic> Characteristics = [];

    public string? MicrochipCode { get; private set; }

    public bool Deleted { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static Animal Create(
        string name,
        string breed,
        Gender gender,
        long price,
        City city,
        Address address,
        List<AppearanceCharacteristic>? characteristics = null,
        string? microchipCode = null)
    {
        characteristics ??= [];

        return new Animal(Guid.NewGuid())
        {
            Name = name,
            Breed = breed,
            Gender = gender,
            Price = price,
            City = city,
            Address = address,
            Characteristics = characteristics,
            MicrochipCode = microchipCode,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public void Update(
        string name,
        string breed,
        Gender gender,
        long price,
        City city,
        Address address,
        List<AppearanceCharacteristic>? characteristics = null,
        string? microchipCode = null)
    {
        characteristics ??= [];

        Name = name;
        Breed = breed;
        Gender = gender;
        Price = price;
        City = city;
        Address = address;
        Characteristics = characteristics;
        MicrochipCode = microchipCode;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        Deleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}
