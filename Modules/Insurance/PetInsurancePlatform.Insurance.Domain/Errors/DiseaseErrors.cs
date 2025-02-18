using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class DiseaseErrors
{
    public static readonly ValidationError EmptyName = new("The name of disease is required.");

    public static readonly string NotCreated = "The disease isn't created.";

    public static string NotFound(Guid id)
    {
        return $"The disease with ID = {id} isn't found.";
    }

    public static string NotUpdated(Guid id)
    {
        return $"The disease with ID = {id} isn't updated.";
    }

    public static string NotRemoved(Guid id)
    {
        return $"The disease with ID = {id} isn't removed.";
    }
}
