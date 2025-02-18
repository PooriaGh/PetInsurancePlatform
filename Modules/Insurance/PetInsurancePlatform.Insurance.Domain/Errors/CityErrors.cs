using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class CityErrors
{
    public static readonly ValidationError EmptyName = new("The name of city is required.");
}
