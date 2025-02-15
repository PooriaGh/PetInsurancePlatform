using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class InsurancePlan : Entity
{
    public static readonly InsurancePlan None = new();

    // Used by EF Core
    private InsurancePlan() : base()
    {
    }

    public string Name { get; set; } = string.Empty;

    private readonly List<InsurancePolicy> _policies = [];
    public IReadOnlyCollection<InsurancePolicy> Policies => _policies.AsReadOnly();

    public static InsurancePlan Create(string name)
    {
        return new InsurancePlan
        {
            Name = name,
        };
    }
}
