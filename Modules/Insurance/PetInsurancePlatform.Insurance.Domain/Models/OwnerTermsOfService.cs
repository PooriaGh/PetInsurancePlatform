using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class OwnerTermsOfService : Entity
{
    // Used by EF Core
    private OwnerTermsOfService()
    {
    }

    public DateTime? AcceptedAt { get; private set; }

    public TermsOfService TermsOfService { get; private set; } = TermsOfService.None;

    public Owner Owner { get; private set; } = Owner.None;

    internal static Result<OwnerTermsOfService> Create(
        TermsOfService termsOfService,
        Owner owner)
    {
        if (termsOfService is null || termsOfService == TermsOfService.None)
        {
            return Result.Invalid(TermsOfServiceErrors.Empty);
        }

        if (owner is null || owner == Owner.None)
        {
            return Result.Invalid(OwnerErrors.Empty);
        }

        return new OwnerTermsOfService
        {
            AcceptedAt = DateTime.UtcNow,
            TermsOfService = termsOfService,
            Owner = owner,
        };
    }
}
