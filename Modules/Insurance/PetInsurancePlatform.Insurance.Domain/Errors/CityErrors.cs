using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class CityErrors
{
    public static readonly ValidationError EmptyName = new("The name of city is required.");

    public static readonly string NotCreated = "The city isn't created.";

    public static string NotFound(Guid id)
    {
        return $"The city with ID = {id} isn't found.";
    }

    public static string NotUpdated(Guid id)
    {
        return $"The city with ID = {id} isn't updated.";
    }

    public static string NotRemoved(Guid id)
    {
        return $"The city with ID = {id} isn't removed.";
    }

    public static string SameName(string name)
    {
        return $"There is already a city with name = {name}.";
    }
}
