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

    public InsurancePlan Plan { get; private set; } = InsurancePlan.None;

    public Pet? Pet { get; private set; }

    public Payment? Payment { get; private set; }

    public TermsOfService? TermsOfService { get; private set; }

    private InsurancePolicyStatus _status;
    public InsurancePolicyStatus Status
    {
        get => ExpiredAt.HasValue && DateTime.Compare(ExpiredAt.Value, DateTime.UtcNow) > 0
            ? InsurancePolicyStatus.Expired
            : _status;
        private set => _status = value;
    }

    public DateTime CreatedAt { get; private set; }

    public DateTime? AcceptedAt { get; private set; }

    public DateTime? PaidAt { get; private set; }

    public DateTime? IssuedAt { get; private set; }

    public DateTime? ExpiredAt { get; private set; }

    internal static Result<InsurancePolicy> Create(InsurancePlan plan)
    {
        if (plan is null || plan == InsurancePlan.None)
        {
            return Result.Invalid(InsurancePolicyErrors.EmptyInsurancePlan);
        }

        return new InsurancePolicy
        {
            Code = Guid.NewGuid().ToString("N"),
            Plan = plan,
            Status = InsurancePolicyStatus.PaymentPending,
            CreatedAt = DateTime.UtcNow,
        };
    }

    internal Result AddPet(Pet pet)
    {
        if (pet is null || pet == Pet.None)
        {
            return Result.Invalid(InsurancePolicyErrors.EmptyPet);
        }

        Pet = pet;

        return Result.Success();
    }

    internal Result AcceptTermsOfService(TermsOfService termsOfService)
    {
        if (termsOfService is null || termsOfService == TermsOfService.None)
        {
            return Result.Invalid(PetErrors.EmptyTermsOfService);
        }

        if (TermsOfService is null || TermsOfService != TermsOfService.None)
        {
            return Result.Invalid(PetErrors.DuplicateTermsOfService);
        }

        TermsOfService = termsOfService;
        AcceptedAt = DateTime.UtcNow;

        return Result.Success();
    }

    internal Result Pay(Payment payment)
    {
        if (payment is null || payment == Payment.None)
        {
            return Result.Invalid(InsurancePolicyErrors.EmptyPayment);
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
