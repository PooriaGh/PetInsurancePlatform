using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class PetTypeErrors
{
    public static readonly ValidationError EmptyName = new("The name of pet type is required.");

    public static readonly ValidationError EmptyDisease = new("The disease of pet type is required.");

    public static readonly string NotCreated = "The pet type isn't created.";

    public static string NotFound(Guid id)
    {
        return $"The pet type with ID = {id} isn't found.";
    }

    public static string NotUpdated(Guid id)
    {
        return $"The pet type with ID = {id} isn't updated.";
    }

    public static string NotRemoved(Guid id)
    {
        return $"The pet type with ID = {id} isn't removed.";
    }

    public static string DuplicateDisease(string name)
    {
        return $"The disease with name = {name} is already added to the pet type.";
    }

    public static string NotFoundDisease(Guid id)
    {
        return $"The pet type doesn't have a disease with ID = {id}.";
    }
}
