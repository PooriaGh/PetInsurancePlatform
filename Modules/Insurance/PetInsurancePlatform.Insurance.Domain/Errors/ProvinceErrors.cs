using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class ProvinceErrors
{
    public static readonly ValidationError EmptyName = new("The name of province is required.");

    public static readonly ValidationError NotRemovable = new("The province with cities can not be removed.");
}
