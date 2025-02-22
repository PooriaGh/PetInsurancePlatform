using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetInsurancePlatform.Insurance.Application.Data;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.SharedKernel.Messaging;

namespace PetInsurancePlatform.Insurance.Application.InsurancePlans.Commands;

public sealed class AddInsurancePolicyCommand(Guid insurancePlanId) : ICommandWithResult<Guid>
{
    public Guid InsurancePlanId { get; set; } = insurancePlanId;

    internal sealed class RequestValidator : Validator<AddInsurancePolicyCommand>
    {
        public RequestValidator()
        {
            RuleFor(req => req.InsurancePlanId)
                .NotEmpty();
        }
    }

    internal sealed class Handler(
        IInsuranceDbContext dbContext,
        ILogger<Handler> logger) : ICommandWithResultHandler<AddInsurancePolicyCommand, Guid>
    {
        public async Task<Result<Guid>> ExecuteAsync(AddInsurancePolicyCommand request, CancellationToken cancellationToken)
        {
            var plan = await dbContext.InsurancePlans
                .Include(pt => pt.Policies)
                .FirstOrDefaultAsync(pt => pt.Id == request.InsurancePlanId, cancellationToken);

            if (plan is null)
            {
                return Result.NotFound(InsurancePlanErrors.NotFound(request.InsurancePlanId));
            }

            var policy = plan.CreatePolicy();

            dbContext.InsurancePolicies.Add(policy);

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);

                return policy.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while selecting the insurance plan.");

                return Result.Error(PetErrors.NotCreated);
            }
        }
    }
}
