using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class InsurancePlanErrors
{
    public static readonly ValidationError Empty = new("The insurance plan is required.");

    public static readonly ValidationError EmptyName = new("The name of insurance plan is required.");

    public static readonly string NotCreated = "The insurance plan isn't created.";

    public static string NotFound(Guid id)
    {
        return $"The insurance plan with ID = {id} isn't found.";
    }

    public static string NotUpdated(Guid id)
    {
        return $"The insurance plan with ID = {id} isn't updated.";
    }

    public static string NotRemoved(Guid id)
    {
        return $"The insurance plan with ID = {id} isn't removed.";
    }

    public static string SameName(string name)
    {
        return $"There is already an insurance plan with name = {name}";
    }
}
