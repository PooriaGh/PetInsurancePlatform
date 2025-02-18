using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Enums;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class InsurancePolicyErrors
{
    public static readonly ValidationError EmptyPet = new("The pet of insurance policy is required.");

    public static readonly ValidationError EmptyInsurancePlan = new("The plan of insurance policy is required.");

    public static readonly ValidationError EmptyPayment = new("The payment of insurance policy is required.");

    public static string NotFoundPolicy(InsurancePolicyStatus status, string name)
    {
        return $"There aren't any policies with status = {status} for the pet with name = {name}.";
    }
}
