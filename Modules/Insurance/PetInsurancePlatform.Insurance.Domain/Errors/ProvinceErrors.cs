using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class ProvinceErrors
{
    public static readonly ValidationError EmptyName = new("The name of insurance plan is required.");

    public static readonly ValidationError NotRemovable = new("The provice with cities can not be removed.");

    public static string DuplicateCity(string name)
    {
        return $"The provice already has the city with name = {name}.";
    }

    public static string NotFoundCity(Guid id)
    {
        return $"The provice doesn't have a city with ID = {id}.";
    }
}
