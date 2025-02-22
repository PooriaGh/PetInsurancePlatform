using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class OwnerErrors
{
    public static readonly ValidationError Empty = new("The owner is required.");

    public static readonly ValidationError EmptyId = new("The ID of owner is required.");

    public static readonly ValidationError EmptyFirstName = new("The first name of owner is required.");

    public static readonly ValidationError EmptyLastName = new("The last name of owner is required.");

    public static readonly ValidationError InvalidNationalID = new("The national ID of owner must have a positive value.");

    public static readonly ValidationError WrongLengthNationalID = new("The national ID of owner must have 10 digits.");

    public static readonly string NotCreated = "The owner isn't created.";

    public static string NotFound(Guid id)
    {
        return $"The owner with ID = {id} isn't found.";
    }

    public static string NotUpdated(Guid id)
    {
        return $"The owner with ID = {id} isn't updated.";
    }

    public static string NotRemoved(Guid id)
    {
        return $"The owner with ID = {id} isn't removed.";
    }
}
