using FastEndpoints.Testing;
using PetInsurancePlatform.Insurance.Endpoints.InsurancePlans.AddPet;

namespace PetInsurancePlatform.Insurance.Tests.InsurancePlans.AddPet;

public class State : StateFixture
{
    public AddPetRequest Request { get; private set; } = null!;
}
