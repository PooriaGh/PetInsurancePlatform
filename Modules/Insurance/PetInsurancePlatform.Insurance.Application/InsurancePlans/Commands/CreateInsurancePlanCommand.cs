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

namespace PetInsurancePlatform.Insurance.Application.InsurancePlans.Commands;

public sealed class CreateInsurancePlanCommand(InsurancePlanRequestDto request) : ICommandWithResult<Guid>
{
    public InsurancePlanRequestDto Request { get; set; } = request;

    internal sealed class RequestValidator : Validator<CreateInsurancePlanCommand>
    {
        public RequestValidator()
        {
            RuleFor(req => req.Request)
                .NotEmpty();

            RuleFor(req => req.Request.Name)
                .NotEmpty();

            RuleFor(req => req.Request.Price)
                .NotEmpty();

            RuleFor(req => req.Request.Coverages)
                .NotEmpty();
        }
    }

    internal sealed class Handler(
        IInsuranceDbContext dbContext,
        ILogger<Handler> logger) : ICommandWithResultHandler<CreateInsurancePlanCommand, Guid>
    {
        public async Task<Result<Guid>> ExecuteAsync(CreateInsurancePlanCommand command, CancellationToken cancellationToken)
        {
            if (await dbContext.InsurancePlans
                .AnyAsync(plan => plan.Name == command.Request.Name, cancellationToken))
            {
                return Result.NotFound(InsurancePlanErrors.SameName(command.Request.Name));
            }

            var price = Money.Create(command.Request.Price);

            if (!price.IsSuccess)
            {
                return Result.Error(new ErrorList(price.Errors));
            }

            var plan = InsurancePlan.Create(
                command.Request.Name,
                command.Request.VIP,
                price);

            if (!plan.IsSuccess)
            {
                return Result.Error(new ErrorList(plan.Errors));
            }

            dbContext.InsurancePlans.Add(plan);

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);

                return plan.Value.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating the insurance plan.");

                return Result.Error(PetErrors.NotCreated);
            }
        }
    }
}
