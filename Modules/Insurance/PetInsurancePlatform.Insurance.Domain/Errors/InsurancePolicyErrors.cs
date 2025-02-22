using PetInsurancePlatform.Insurance.Domain.Enums;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class InsurancePolicyErrors
{
    public static readonly string NotCreated = "The insurance policy isn't created.";

    public static string NotFound(Guid id)
    {
        return $"The insurance policy with ID = {id} isn't found.";
    }

    public static string NotFound(InsurancePolicyStatus status)
    {
        return $"The insurance policy with status = {status} isn't found.";
    }

    public static string NotUpdated(Guid id)
    {
        return $"The insurance policy with ID = {id} isn't updated.";
    }

    public static string NotRemoved(Guid id)
    {
        return $"The insurance policy with ID = {id} isn't removed.";
    }

    public static string SameStatus(InsurancePolicyStatus status)
    {
        return $"There is already an insurance policy with status = {status}";
    }
}
