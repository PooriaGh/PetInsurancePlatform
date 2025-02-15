using PetInsurancePlatform.Insurance.Domain.Enums;
using PetInsurancePlatform.Insurance.Domain.ValueObjects;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class Pet : Entity
{
    public static readonly Pet None = new();

    // Used by EF Core
    private Pet() : base()
    {
    }

    public string Name { get; private set; } = null!;

    public string Breed { get; private set; } = null!;

    public Gender Gender { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public PetType Type { get; private set; } = null!;

    public long Price { get; private set; }

    public City City { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    public Owner Owner { get; private set; } = null!;

    public IReadOnlyCollection<AppearanceCharacteristic> Characteristics = [];

    public string? MicrochipCode { get; private set; }

    public bool Deleted { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static Pet Create(
        string name,
        string breed,
        Gender gender,
        DateOnly dateOfBirth,
        PetType type,
        long price,
        City city,
        Address address,
        Owner owner,
        List<AppearanceCharacteristic>? characteristics = null,
        string? microchipCode = null)
    {
        characteristics ??= [];

        return new Pet
        {
            Name = name,
            Breed = breed,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            Type = type,
            Price = price,
            City = city,
            Address = address,
            Owner = owner,
            Characteristics = characteristics,
            MicrochipCode = microchipCode,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public void Update(
        string name,
        string breed,
        Gender gender,
        DateOnly dateOfBirth,
        PetType type,
        long price,
        City city,
        Address address,
        Owner owner,
        List<AppearanceCharacteristic>? characteristics = null,
        string? microchipCode = null)
    {
        characteristics ??= [];

        Name = name;
        Breed = breed;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        Type = type;
        Price = price;
        City = city;
        Address = address;
        Owner = owner;
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
