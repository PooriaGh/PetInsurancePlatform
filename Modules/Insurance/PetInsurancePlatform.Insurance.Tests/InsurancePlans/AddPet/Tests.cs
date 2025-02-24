using FastEndpoints;
using FastEndpoints.Testing;
using PetInsurancePlatform.Insurance.Application.Dtos;
using PetInsurancePlatform.Insurance.Domain.Enums;
using PetInsurancePlatform.Insurance.Endpoints.InsurancePlans.AddPet;
using Shouldly;
using System.Net;

namespace PetInsurancePlatform.Insurance.Tests.InsurancePlans.AddPet;

public class Tests(Sut App) : TestBase<Sut>
{
    [Fact, Priority(1)]
    public async Task Invalid_Pet_Request_Input()
    {
        var (response, result) = await App.Client.POSTAsync<AddPetEndpoint, AddPetRequest, Guid>(new()
        {
            InsurancePlanId = Guid.NewGuid(),
            InsurancePolicyId = Guid.NewGuid(),
            PetRequest = new PetRequestDto
            {
                Name = "",
                Breed = "",
                Gender = Gender.Male,
                DateOfBirth = new DateOnly(),
                PetTypeId = Guid.NewGuid(),
                Price = 0,
                CityId = Guid.NewGuid(),
                Address = new AddressDto
                {
                    District = 20,
                    Street = "",
                    Alley = "",
                    PlateNumber = 10,
                    PostalCode = 10,
                },
                Appearances = [],
                MicrochipCode = "",
                DiseasesIds = [Guid.NewGuid()]
            }
        });

        //var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldBeOfType<Guid>();
        Assert.NotEqual(Guid.Empty, result);
    }
}
