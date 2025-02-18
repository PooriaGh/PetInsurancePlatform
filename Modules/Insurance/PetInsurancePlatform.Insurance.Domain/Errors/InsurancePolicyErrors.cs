using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Enums;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class InsurancePolicyErrors
{
    public static readonly ValidationError EmptyPet = new("The pet of insurance policy is required.");

    public static readonly ValidationError EmptyInsurancePlan = new("The plan of insurance policy is required.");

    public static readonly ValidationError EmptyPayment = new("The payment of insurance policy is required.");

    public static readonly string NotCreated = "The insurance policy isn't created.";

    public static string NotFound(Guid id)
    {
        return $"The insurance policy with ID = {id} isn't found.";
    }

    public static string NotUpdated(Guid id)
    {
        return $"The insurance policy with ID = {id} isn't updated.";
    }

    public static string NotRemoved(Guid id)
    {
        return $"The insurance policy with ID = {id} isn't removed.";
    }

    public static string NotFoundPolicy(InsurancePolicyStatus status, string name)
    {
        return $"There aren't any policies with status = {status} for the pet with name = {name}.";
    }
}
