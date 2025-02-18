using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class DiseaseErrors
{
    public static readonly ValidationError EmptyName = new("The name of disease is required.");
}
