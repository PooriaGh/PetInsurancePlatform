using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetInsurancePlatform.Insurance.Application.Data;
using PetInsurancePlatform.Insurance.Application.Dtos;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.Insurance.Domain.ValueObjects;

namespace PetInsurancePlatform.Insurance.Application.InsurancePlans.Commands;

public sealed class AddPetToInsurancePolicyCommand(
    Guid insurancePlanId,
    Guid insurancePolicyId,
    PetRequestDto request) : ICommand<Result>
{
    public Guid InsurancePlanId { get; set; } = insurancePlanId;

    public Guid InsurancePolicyId { get; set; } = insurancePolicyId;

    public PetRequestDto Request { get; set; } = request;

    internal sealed class RequestValidator : Validator<AddPetToInsurancePolicyCommand>
    {
        public RequestValidator()
        {
            RuleFor(req => req.InsurancePlanId)
                .NotEmpty();

            RuleFor(req => req.InsurancePolicyId)
               .NotEmpty();

            RuleFor(req => req.Request)
                .NotEmpty();

            RuleFor(req => req.Request.Name)
                .NotEmpty();

            RuleFor(req => req.Request.Breed)
                .NotEmpty();

            RuleFor(req => req.Request.Gender)
                .IsInEnum();

            RuleFor(req => req.Request.DateOfBirth)
                .NotEmpty();

            RuleFor(req => req.Request.PetTypeId)
                .NotEmpty();

            RuleFor(req => req.Request.Price)
                .NotEmpty();

            RuleFor(req => req.Request.CityId)
                .NotEmpty();

            RuleFor(req => req.Request.Address)
                .NotEmpty();

            RuleFor(req => req.Request.Appearances)
                .NotEmpty();

            RuleFor(req => req.Request.MicrochipCode)
                .NotEmpty();
        }
    }

    public class AddPetCommandHandler(
        IInsuranceDbContext dbContext,
        ILogger<AddPetCommandHandler> logger) : ICommandHandler<AddPetToInsurancePolicyCommand, Result>
    {
        public async Task<Result> ExecuteAsync(AddPetToInsurancePolicyCommand command, CancellationToken ct)
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

            var petType = await dbContext.PetTypes
                .Include(p => p.Diseases)
                .FirstOrDefaultAsync(pt => pt.Id == command.Request.PetTypeId, ct);

            if (petType is null)
            {
                return Result.NotFound(PetTypeErrors.NotFound(command.Request.PetTypeId));
            }

            var city = await dbContext.Cities
                .FirstOrDefaultAsync(pt => pt.Id == command.Request.CityId, ct);

            if (city is null)
            {
                return Result.NotFound(CityErrors.NotFound(command.Request.CityId));
            }

            foreach (var diseasesId in command.Request.DiseasesIds)
            {
                if (!await dbContext.Diseases
                    .AnyAsync(d => d.Id == diseasesId, ct))
                {
                    return Result.NotFound(DiseaseErrors.NotFound(diseasesId));
                }
            }

            var diseases = await dbContext.Diseases
                .Where(d => command.Request.DiseasesIds.Contains(d.Id))
                .ToListAsync(ct);

            if (city is null)
            {
                return Result.NotFound(CityErrors.NotFound(command.Request.CityId));
            }

            var price = Money.Create(command.Request.Price);

            if (!price.IsSuccess)
            {
                return Result.Error(new ErrorList(price.Errors));
            }

            var address = Address.Create(
                command.Request.Address.District,
                command.Request.Address.Street,
                command.Request.Address.Alley,
                command.Request.Address.PlateNumber,
                command.Request.Address.PostalCode);

            if (!address.IsSuccess)
            {
                return Result.Error(new ErrorList(address.Errors));
            }

            var appearances = command.Request.Appearances
                .Select(Appearance.Create);

            if (!appearances.Any(a => !a.IsSuccess))
            {
                return Result.Error(new ErrorList(appearances.SelectMany(a => a.Errors)));
            }

            var result = plan.AddPetToPolicy(
                policy,
                command.Request.Name,
                command.Request.Breed,
                command.Request.Gender,
                command.Request.DateOfBirth,
                petType,
                price,
                city,
                address,
                appearances.Select(a => a.Value),
                command.Request.MicrochipCode,
                diseases);

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
    }
}
