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
using PetInsurancePlatform.SharedKernel.Messaging;

namespace PetInsurancePlatform.Insurance.Application.Pets.Commands;

public sealed class CreatePetCommand(PetRequestDto petRequest) : ICommandWithResult<Guid>
{
    public PetRequestDto PetRequest { get; set; } = petRequest;

    internal sealed class RequestValidator : Validator<CreatePetCommand>
    {
        public RequestValidator()
        {
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

    internal sealed class Handler(
        IInsuranceDbContext dbContext,
        ILogger<Handler> logger) : ICommandWithResultHandler<CreatePetCommand, Guid>
    {
        public async Task<Result<Guid>> ExecuteAsync(CreatePetCommand request, CancellationToken cancellationToken)
        {
            var petType = await dbContext.PetTypes
                .Include(p => p.Diseases)
                .FirstOrDefaultAsync(pt => pt.Id == request.PetRequest.PetTypeId, cancellationToken);

            if (petType is null)
            {
                return Result.NotFound(PetTypeErrors.NotFound(request.PetRequest.PetTypeId));
            }

            var city = await dbContext.Cities
                .FirstOrDefaultAsync(pt => pt.Id == request.PetRequest.CityId, cancellationToken);

            if (city is null)
            {
                return Result.NotFound(CityErrors.NotFound(request.PetRequest.CityId));
            }

            foreach (var diseasesId in request.PetRequest.DiseasesIds)
            {
                if (!await dbContext.Diseases
                    .AnyAsync(d => d.Id == diseasesId, cancellationToken))
                {
                    return Result.NotFound(DiseaseErrors.NotFound(diseasesId));
                }
            }

            var diseases = await dbContext.Diseases
                .Where(d => request.PetRequest.DiseasesIds.Contains(d.Id))
                .ToListAsync(cancellationToken);

            if (city is null)
            {
                return Result.NotFound(CityErrors.NotFound(request.PetRequest.CityId));
            }

            var price = Money.Create(request.PetRequest.Price);

            if (!price.IsSuccess)
            {
                return Result.Error(new ErrorList(price.Errors));
            }

            var address = Address.Create(
                request.PetRequest.Address.District,
                request.PetRequest.Address.Street,
                request.PetRequest.Address.Alley,
                request.PetRequest.Address.PlateNumber,
                request.PetRequest.Address.PostalCode);

            if (!address.IsSuccess)
            {
                return Result.Error(new ErrorList(address.Errors));
            }

            var appearances = request.PetRequest.Appearances
                .Select(Appearance.Create);

            if (!appearances.Any(a => !a.IsSuccess))
            {
                return Result.Error(new ErrorList(appearances.SelectMany(a => a.Errors)));
            }

            var pet = Pet.Create(
                request.PetRequest.Name,
                request.PetRequest.Breed,
                request.PetRequest.Gender,
                request.PetRequest.DateOfBirth,
                petType,
                price.Value,
                city,
                address.Value,
                [.. appearances.Select(res => res.Value)],
                request.PetRequest.MicrochipCode,
                diseases);

            dbContext.Pets.Add(pet.Value);

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);

                return pet.Value.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating the pet.");

                return Result.Error(PetErrors.NotCreated);
            }
        }
    }
}
