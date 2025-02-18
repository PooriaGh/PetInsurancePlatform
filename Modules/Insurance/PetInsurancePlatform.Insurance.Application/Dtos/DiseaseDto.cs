namespace PetInsurancePlatform.Insurance.Application.Dtos;

public sealed class DiseaseDto
{
    public static readonly DiseaseDto None = new();

    public string Name { get; private set; } = string.Empty;

    public bool Accepted { get; private set; }
}
