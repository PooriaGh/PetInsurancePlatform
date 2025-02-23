﻿using Ardalis.Result;
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

    public List<InsuranceCoverage> Coverages { get; } = [];

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    private readonly List<InsurancePolicy> _policies = [];
    public IReadOnlyCollection<InsurancePolicy> Policies => _policies.AsReadOnly();

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
            if (Coverages.Any(c => c == coverage))
            {
                return Result.Conflict(InsuranceCoverageErrors.SameName(coverage.Name));
            }
        }

        Coverages.AddRange(coverages);

        return Result.Success();
    }

    public InsurancePolicy CreatePolicy()
    {
        var policy = InsurancePolicy.Create();

        _policies.Add(policy);

        return policy;
    }

    public Result AddPetToPolicy(
        InsurancePolicy policy,
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
        if (_policies
            .Any(p => p.Status == InsurancePolicyStatus.Active))
        {
            return Result.Conflict(InsurancePolicyErrors.SameStatus(InsurancePolicyStatus.Active));
        }

        if (!_policies.Any(p => p == policy))
        {
            return Result.NotFound(InsurancePolicyErrors.NotFound(policy.Id));
        }

        var result = policy.AddPet(
            name,
            breed,
            gender,
            dateOfBirth,
            price,
            address,
            appearances,
            microchipCode,
            petType,
            city,
            diseases);

        if (!result.IsSuccess)
        {
            return Result.Error(new ErrorList(result.Errors));
        }

        return Result.Success();
    }

    public Result PayPolicy(
        Pet pet,
        Payment payment)
    {
        var policy = _policies
            .Where(p => p.Pet is not null)
            .FirstOrDefault(p => p.Pet! == pet && p.Status == InsurancePolicyStatus.PaymentPending);

        if (policy is null)
        {
            return Result.NotFound(InsurancePolicyErrors.NotFound(InsurancePolicyStatus.PaymentPending));
        }

        var result = policy.Pay(payment);

        if (!result.IsSuccess)
        {
            return Result.Error(new ErrorList(result.Errors));
        }

        return Result.Success();
    }

    public Result IssuePolicy(Pet pet)
    {
        var policy = _policies
            .Where(p => p.Pet is not null)
            .FirstOrDefault(p => p.Pet! == pet && p.Status == InsurancePolicyStatus.HealthCertificatePending);

        if (policy is null)
        {
            return Result.NotFound(InsurancePolicyErrors.NotFound(InsurancePolicyStatus.PaymentPending));
        }

        policy.Issue();

        return Result.Success();
    }
}
