using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class TermsOfService : Entity
{
    public static readonly TermsOfService None = new();

    // Used by EF Core
    private TermsOfService()
    {
    }

    public string Text { get; private set; } = string.Empty;

    public int Version { get; private set; }

    public static Result<TermsOfService> Create(
        string text,
        int version)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Result.Invalid(TermsOfServiceErrors.EmptyName);
        }

        if (version == 0)
        {
            return Result.Invalid(TermsOfServiceErrors.EmptyVersion);
        }

        if (version < 0)
        {
            return Result.Invalid(TermsOfServiceErrors.InvalidVersion);
        }

        return new TermsOfService
        {
            Text = text,
            Version = version,
        };
    }
}
