using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Enums;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class InsurancePlanErrors
{
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

    public static string DuplicateCoverage(string name)
    {
        return $"The plan already has the coverage with name = {name}.";
    }

    public static string DuplicatePlan(string name)
    {
        return $"The pet already has an active insurance policy of the plan with name = {name}.";
    }

    public static string DuplicatePlan(InsurancePolicyStatus status)
    {
        return $"There aren't any policies with status = {status} for the current pet.";
    }
}
