using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class InsurancePolicy : Entity
{
    // Used by EF Core
    private InsurancePolicy() : base()
    {
    }

    public Pet Pet { get; private set; } = Pet.None;

    public InsurancePlan Plan { get; private set; } = InsurancePlan.None;


    public DateTime? IssuedAt { get; private set; }

    public DateTime? ExpiredAt { get; private set; }

    public static InsurancePolicy Issue(Pet pet, int policyPeriodsInDays)
    {
        return new InsurancePolicy
        {
            Pet = pet,
            IssuedAt = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.AddDays(policyPeriodsInDays),
        };
    }
}
