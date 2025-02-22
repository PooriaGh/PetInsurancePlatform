using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Enums;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.Insurance.Domain.ValueObjects;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class InsurancePolicy : Entity
{
    // Used by EF Core
    private InsurancePolicy() : base()
    {
    }

    public string Code { get; set; } = string.Empty;

    public Pet? Pet { get; private set; }

    public Payment Payment { get; private set; } = Payment.None;

    private InsurancePolicyStatus _status;
    public InsurancePolicyStatus Status
    {
        get => ExpiredAt.HasValue && DateTime.Compare(ExpiredAt.Value, DateTime.UtcNow) > 0
            ? InsurancePolicyStatus.Expired
            : _status;
        private set => _status = value;
    }

    public DateTime CreatedAt { get; private set; }

    public DateTime? PaidAt { get; private set; }

    public DateTime? IssuedAt { get; private set; }

    public DateTime? ExpiredAt { get; private set; }

    internal static InsurancePolicy Create()
    {
        return new InsurancePolicy
        {
            Code = Guid.NewGuid().ToString("N"),
            Status = InsurancePolicyStatus.PaymentPending,
            CreatedAt = DateTime.UtcNow,
        };
    }

    internal Result AddPet(
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
        var pet = Pet.Create(
            name,
            breed,
            gender,
            dateOfBirth,
            petType,
            price,
            city,
            address,
            appearances,
            microchipCode,
            diseases);

        if (!pet.IsSuccess)
        {
            return Result.Error(new ErrorList([.. pet.Errors]));
        }

        Pet = pet;

        return Result.Success();
    }

    public Result EditPet(
        string name,
        string breed,
        Gender gender,
        DateOnly dateOfBirth,
        PetType petType,
        Money price,
        City city,
        Address address,
        List<Appearance> appearances,
        string microchipCode,
        List<Disease> diseases,
        List<StoredFile> birthCertificatesPages,
        StoredFile frontView,
        StoredFile rearView,
        StoredFile rightSideView,
        StoredFile leftSideView,
        StoredFile walkingVideo,
        StoredFile healthCertificate)
    {
        if (Pet is null || Pet == Pet.None)
        {
            return Result.NotFound(PetErrors.NotFound());
        }

        return Pet.Update(
            name,
            breed,
            gender,
            dateOfBirth,
            petType,
            price,
            city,
            address,
            appearances,
            microchipCode,
            diseases,
            birthCertificatesPages,
            frontView,
            rearView,
            rightSideView,
            leftSideView,
            walkingVideo,
            healthCertificate);
    }

    internal Result Pay(Payment payment)
    {
        if (payment is null || payment == Payment.None)
        {
            return Result.Invalid(Payment.Empty);
        }

        Status = InsurancePolicyStatus.HealthCertificatePending;
        PaidAt = DateTime.UtcNow;

        return Result.Success();
    }

    internal void Issue()
    {
        Status = InsurancePolicyStatus.Active;
        IssuedAt = DateTime.UtcNow;
        ExpiredAt = DateTime.UtcNow.AddYears(1);
    }
}
