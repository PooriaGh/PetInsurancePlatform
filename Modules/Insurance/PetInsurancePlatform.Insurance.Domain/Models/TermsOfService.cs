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

    public static TermsOfService Create(
        string text,
        int version)
    {
        return new TermsOfService
        {
            Text = text,
            Version = version,
        };
    }
}
