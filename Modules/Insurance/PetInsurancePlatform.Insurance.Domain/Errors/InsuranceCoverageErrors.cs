namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class InsuranceCoverageErrors
{
    public static string SameName(string name)
    {
        return $"There is already an insurance coverage with name = {name}";
    }
}
