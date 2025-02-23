using PetInsurancePlatform.Insurance.Application.Dtos;

namespace PetInsurancePlatform.Insurance.Endpoints.InsurancePlans.AddPet;

public sealed class AddPetRequest
{
    public Guid InsurancePlanId { get; set; }

    public Guid InsurancePolicyId { get; set; }

    public PetRequestDto PetRequest { get; set; } = PetRequestDto.None;
}
