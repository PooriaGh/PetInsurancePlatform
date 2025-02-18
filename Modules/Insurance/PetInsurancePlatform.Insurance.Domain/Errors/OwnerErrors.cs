using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class OwnerErrors
{
    public static readonly ValidationError EmptyFirstName = new("The first name of owner is required.");

    public static readonly ValidationError EmptyLastName = new("The last name of owner is required.");

    public static readonly ValidationError InvalidNationalID = new("The national ID of owner must have a positive value.");

    public static readonly ValidationError WrongLengthNationalID = new("The national ID of owner must have 10 digits.");
}
