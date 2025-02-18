using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Enums;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.Insurance.Domain.ValueObjects;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class InsurancePlan : Entity
{
    public static readonly InsurancePlan None = new();

    // Used by EF Core
    private InsurancePlan() : base()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public bool VIP { get; private set; }

    public Money Price { get; private set; } = Money.Zero;

    private readonly List<InsurancePolicy> _policies = [];
    public IReadOnlyCollection<InsurancePolicy> Policies => _policies.AsReadOnly();

    private readonly List<InsuranceCoverage> _coverages = [];
    public IReadOnlyCollection<InsuranceCoverage> Coverages => _coverages.AsReadOnly();

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public static Result<InsurancePlan> Create(
        string name,
        bool vip,
        Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Invalid(InsurancePlanErrors.EmptyName);
        }

        return new InsurancePlan
        {
            Name = name,
            VIP = vip,
            Price = price,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public Result Update(
       string name,
       bool vip,
       Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Invalid(InsurancePlanErrors.EmptyName);
        }

        Name = name;
        VIP = vip;
        Price = price;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result AddCoverages(params InsuranceCoverage[] coverages)
    {
        foreach (var coverage in coverages)
        {
            if (_coverages.Any(c => c == coverage))
            {
                return Result.Conflict(InsurancePlanErrors.DuplicateCoverage(coverage.Name));
            }
        }

        _coverages.AddRange(coverages);

        return Result.Success();
    }

    public Result CreatePolicy(Pet pet)
    {
        if (_policies.Any(p => p.Pet == pet && p.Status == InsurancePolicyStatus.Active))
        {
            return Result.Conflict(InsurancePlanErrors.DuplicatePlan(Name));
        }

        var result = InsurancePolicy.Create(pet, this);

        if (result.IsError())
        {
            return Result.Conflict(result.Errors.ToArray());
        }

        _policies.Add(result.Value);

        return Result.Success();
    }

    public Result PayPolicy(
        Pet pet,
        Payment payment)
    {
        var policy = _policies
            .FirstOrDefault(p => p.Pet == pet && p.Status == InsurancePolicyStatus.PaymentPending);

        if (policy is null)
        {
            return Result.NotFound(InsurancePolicyErrors.NotFoundPolicy(InsurancePolicyStatus.PaymentPending, pet.Name));
        }

        var result = policy.Pay(payment);

        if (result.IsError())
        {
            return Result.Conflict(result.Errors.ToArray());
        }

        return Result.Success();
    }

    public Result IssuePolicy(Pet pet)
    {
        var policy = _policies
            .FirstOrDefault(p => p.Pet == pet && p.Status == InsurancePolicyStatus.HealthCertificatePending);

        if (policy is null)
        {
            return Result.NotFound(InsurancePolicyErrors.NotFoundPolicy(InsurancePolicyStatus.PaymentPending, pet.Name));
        }

        policy.Issue();

        return Result.Success();
    }
}
