using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class PetErrors
{
    public static readonly ValidationError Empty = new("The pet is required.");

    public static readonly string NotCreated = "The pet isn't created.";

    public static string NotFound()
    {
        return $"The pet isn't found.";
    }

    public static string NotFound(Guid id)
    {
        return $"The pet with ID = {id} isn't found.";
    }

    public static string NotUpdated(Guid id)
    {
        return $"The pet with ID = {id} isn't updated.";
    }

    public static string NotRemoved(Guid id)
    {
        return $"The pet with ID = {id} isn't removed.";
    }

    public static string OutOfRangeAge(int age)
    {
        return $"The pet's age with value = {age} isn't in the acceptable range of the insurance company.";
    }

    public static string NotCoveredDisease(string name)
    {
        return $"The diease with name = {name} is not covered by the insurance company.";
    }
}
