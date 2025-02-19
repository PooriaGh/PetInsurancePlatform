using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetInsurancePlatform.Insurance.Application.Data;
using PetInsurancePlatform.Insurance.Application.Dtos;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.Insurance.Domain.Models;
using PetInsurancePlatform.Insurance.Domain.ValueObjects;

namespace PetInsurancePlatform.Insurance.Application.InsurancePlans.Commands;

public sealed class AddPetCommand(
    Guid insurancePlanId,
    Guid insurancePolicyId,
    PetRequestDto petRequest) : ICommand<Result>
{
    public Guid InsurancePlanId { get; set; } = insurancePlanId;

    public Guid InsurancePolicyId { get; set; } = insurancePolicyId;

    public PetRequestDto PetRequest { get; set; } = petRequest;

    internal sealed class RequestValidator : Validator<AddPetCommand>
    {
        public RequestValidator()
        {
            RuleFor(req => req.InsurancePlanId)
                .NotEmpty();

            RuleFor(req => req.InsurancePolicyId)
               .NotEmpty();

            RuleFor(req => req.PetRequest)
                .NotEmpty();

            RuleFor(req => req.PetRequest.Name)
                .NotEmpty();

            RuleFor(req => req.PetRequest.Breed)
                .NotEmpty();

            RuleFor(req => req.PetRequest.Gender)
                .IsInEnum();

            RuleFor(req => req.PetRequest.DateOfBirth)
                .NotEmpty();

            RuleFor(req => req.PetRequest.PetTypeId)
                .NotEmpty();

            RuleFor(req => req.PetRequest.Price)
                .NotEmpty();

            RuleFor(req => req.PetRequest.CityId)
                .NotEmpty();

            RuleFor(req => req.PetRequest.Address)
                .NotEmpty();

            RuleFor(req => req.PetRequest.Appearances)
                .NotEmpty();

            RuleFor(req => req.PetRequest.MicrochipCode)
                .NotEmpty();
        }
    }

    public class AddPetCommandHandler(
        IInsuranceDbContext dbContext,
        ILogger<AddPetCommandHandler> logger) : ICommandHandler<AddPetCommand, Result>
    {
        public async Task<Result> ExecuteAsync(AddPetCommand command, CancellationToken ct)
        {
            var plan = await dbContext.InsurancePlans
                .Include(pt => pt.Policies.Where(p => p.Id == command.InsurancePolicyId))
                .FirstOrDefaultAsync(pt => pt.Id == command.InsurancePlanId, ct);

            if (plan is null)
            {
                return Result.NotFound(InsurancePlanErrors.NotFound(command.InsurancePlanId));
            }

            var policy = plan.Policies
                .FirstOrDefault(p => p.Id == command.InsurancePolicyId);

            if (policy is null)
            {
                return Result.NotFound(InsurancePolicyErrors.NotFound(command.InsurancePlanId));
            }

            var pet = await CreatePetAsync(command.PetRequest, ct);

            if (!pet.IsSuccess)
            {
                return Result.Error(new ErrorList(pet.Errors));
            }

            var result = plan.AddPetToPolicy(pet, policy);

            if (!result.IsSuccess)
            {
                return Result.Error(new ErrorList(result.Errors));
            }

            try
            {
                await dbContext.SaveChangesAsync(ct);

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding pet to the insurance policy.");

                return Result.Error(PetErrors.NotCreated);
            }
        }

        private async Task<Result<Pet>> CreatePetAsync(
            PetRequestDto petRequest,
            CancellationToken cancellationToken)
        {
            var petType = await dbContext.PetTypes
            .Include(p => p.Diseases)
            .FirstOrDefaultAsync(pt => pt.Id == petRequest.PetTypeId, cancellationToken);

            if (petType is null)
            {
                return Result.NotFound(PetTypeErrors.NotFound(petRequest.PetTypeId));
            }

            var city = await dbContext.Cities
                .FirstOrDefaultAsync(pt => pt.Id == petRequest.CityId, cancellationToken);

            if (city is null)
            {
                return Result.NotFound(CityErrors.NotFound(petRequest.CityId));
            }

            foreach (var diseasesId in petRequest.DiseasesIds)
            {
                if (!await dbContext.Diseases
                    .AnyAsync(d => d.Id == diseasesId, cancellationToken))
                {
                    return Result.NotFound(DiseaseErrors.NotFound(diseasesId));
                }
            }

            var diseases = await dbContext.Diseases
                .Where(d => petRequest.DiseasesIds.Contains(d.Id))
                .ToListAsync(cancellationToken);

            if (city is null)
            {
                return Result.NotFound(CityErrors.NotFound(petRequest.CityId));
            }

            var price = Money.Create(petRequest.Price);

            if (!price.IsSuccess)
            {
                return Result.Error(new ErrorList(price.Errors));
            }

            var address = Address.Create(
                petRequest.Address.District,
                petRequest.Address.Street,
                petRequest.Address.Alley,
                petRequest.Address.PlateNumber,
                petRequest.Address.PostalCode);

            if (!address.IsSuccess)
            {
                return Result.Error(new ErrorList(address.Errors));
            }

            var appearances = petRequest.Appearances
                .Select(Appearance.Create);

            if (!appearances.Any(a => !a.IsSuccess))
            {
                return Result.Error(new ErrorList(appearances.SelectMany(a => a.Errors)));
            }

            return Pet.Create(
                petRequest.Name,
                petRequest.Breed,
                petRequest.Gender,
                petRequest.DateOfBirth,
                petType,
                price.Value,
                city,
                address.Value,
                [.. appearances.Select(res => res.Value)],
                petRequest.MicrochipCode,
                diseases);
        }
    }
}
