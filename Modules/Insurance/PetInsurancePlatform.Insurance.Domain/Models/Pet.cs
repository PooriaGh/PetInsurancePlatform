using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Enums;
using PetInsurancePlatform.Insurance.Domain.Errors;
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

    public string Name { get; private set; } = string.Empty;

    public string Breed { get; private set; } = string.Empty;

    public Gender Gender { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public PetType PetType { get; private set; } = PetType.None;

    public Money Price { get; private set; } = Money.Zero;

    public City City { get; private set; } = City.None;

    public Address Address { get; private set; } = Address.None;

    public IReadOnlyCollection<Appearance> Appearances { get; private set; } = [];

    public string MicrochipCode { get; private set; } = string.Empty;

    public IReadOnlyCollection<Disease> Diseases { get; private set; } = [];
    public bool HasDiseases => Diseases.Count != 0;

    public IReadOnlyCollection<StoredFile> BirthCertificatesPages { get; private set; } = [];

    public StoredFile FrontView { get; private set; } = StoredFile.None;

    public StoredFile RearView { get; private set; } = StoredFile.None;

    public StoredFile RightSideView { get; private set; } = StoredFile.None;

    public StoredFile LeftSideView { get; private set; } = StoredFile.None;

    public StoredFile WalkingVideo { get; private set; } = StoredFile.None;

    public StoredFile HealthCertificate { get; private set; } = StoredFile.None;

    public bool Deleted { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    internal static Result<Pet> Create(
        string name,
        string breed,
        Gender gender,
        DateOnly dateOfBirth,
        PetType petType,
        Money price,
        City city,
        Address address,
        IEnumerable<Appearance> appearances,
        string microchipCode,
        IEnumerable<Disease> diseases)
    {
        appearances ??= [];
        diseases ??= [];

        var age = DateTime.UtcNow.Date.Year - dateOfBirth.Year;

        if (petType.AgeRange.MinimumValue > age
            || petType.AgeRange.MaximumValue < age)
        {
            return Result.Conflict(PetErrors.OutOfRangeAge(age));
        }

        foreach (var disease in diseases)
        {
            if (!petType.Diseases.Contains(disease))
            {
                return Result.Conflict(PetErrors.NotCoveredDisease(disease.Name));
            }
        }

        return new Pet
        {
            Name = name,
            Breed = breed,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            PetType = petType,
            Price = price,
            City = city,
            Address = address,
            Appearances = [.. appearances],
            MicrochipCode = microchipCode,
            CreatedAt = DateTime.UtcNow,
            Diseases = [.. diseases],
        };
    }

    internal Result AddImagesAndVideo(
        List<StoredFile> birthCertificatesPages,
        StoredFile frontView,
        StoredFile rearView,
        StoredFile rightSideView,
        StoredFile leftSideView,
        StoredFile walkingVideo)
    {
        if (birthCertificatesPages is null
            || birthCertificatesPages.Count == 0
            || frontView is null
            || frontView == StoredFile.None
            || rearView is null
            || rearView == StoredFile.None
            || rightSideView is null
            || rightSideView == StoredFile.None
            || leftSideView is null
            || leftSideView == StoredFile.None
            || walkingVideo is null
            || walkingVideo == StoredFile.None)
        {
            return Result.Invalid(StoredFile.Empty);
        }

        BirthCertificatesPages = birthCertificatesPages;
        FrontView = frontView;
        RearView = rearView;
        RightSideView = rightSideView;
        LeftSideView = leftSideView;
        WalkingVideo = walkingVideo;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    internal Result AddHealthCertificate(StoredFile healthCertificate)
    {
        if (healthCertificate is null || healthCertificate == StoredFile.None)
        {
            return Result.Invalid(StoredFile.Empty);
        }

        HealthCertificate = healthCertificate;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    internal Result Update(
        string name,
        string breed,
        Gender gender,
        DateOnly dateOfBirth,
        PetType petType,
        Money price,
        City city,
        Address address,
        IEnumerable<Appearance> appearances,
        string microchipCode,
        IEnumerable<Disease> diseases,
        IEnumerable<StoredFile> birthCertificatesPages,
        StoredFile frontView,
        StoredFile rearView,
        StoredFile rightSideView,
        StoredFile leftSideView,
        StoredFile walkingVideo,
        StoredFile healthCertificate)
    {
        appearances ??= [];
        diseases ??= [];

        Name = name;
        Breed = breed;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        PetType = petType;
        Price = price;
        City = city;
        Address = address;
        Appearances = [.. appearances];
        MicrochipCode = microchipCode;
        Diseases = [.. diseases];
        UpdatedAt = DateTime.UtcNow;

        var result = AddImagesAndVideo(
            [.. birthCertificatesPages],
            frontView,
            rearView,
            rightSideView,
            leftSideView,
            walkingVideo);

        if (!result.IsSuccess)
        {
            return result;
        }

        result = AddHealthCertificate(healthCertificate);

        if (!result.IsSuccess)
        {
            return result;
        }

        return Result.Success();
    }

    internal void Delete()
    {
        Deleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}
