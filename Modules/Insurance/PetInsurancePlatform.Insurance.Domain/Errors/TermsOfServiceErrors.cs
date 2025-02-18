using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class TermsOfServiceErrors
{
    public static readonly ValidationError EmptyName = new("The text of terms of service is required.");

    public static readonly ValidationError EmptyVersion = new("The text of terms of service is required.");

    public static readonly ValidationError InvalidVersion = new("The text of terms of service must have a positive value.");
}
