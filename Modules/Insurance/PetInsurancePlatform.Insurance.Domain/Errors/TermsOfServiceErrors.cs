using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class TermsOfServiceErrors
{
    public static readonly ValidationError Empty = new("The terms of service is required.");

    public static readonly ValidationError EmptyName = new("The text of terms of service is required.");

    public static readonly ValidationError EmptyVersion = new("The text of terms of service is required.");

    public static readonly ValidationError InvalidVersion = new("The text of terms of service must have a positive value.");

    public static readonly ValidationError AlreadyAccepted = new("The terms of service is already accepted.");

    public static readonly string NotCreated = "The terms of service isn't created.";

    public static string NotFound(Guid id)
    {
        return $"The terms of service with ID = {id} isn't found.";
    }

    public static string NotUpdated(Guid id)
    {
        return $"The terms of service with ID = {id} isn't updated.";
    }

    public static string NotRemoved(Guid id)
    {
        return $"The terms of service with ID = {id} isn't removed.";
    }
}
