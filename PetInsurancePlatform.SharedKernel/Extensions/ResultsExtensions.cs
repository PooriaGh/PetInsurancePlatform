using Ardalis.Result;
using FastEndpoints;
using FluentValidation.Results;

namespace PetInsurancePlatform.SharedKernel.Extensions;

/// <summary>
/// This is a workaround for the lack of support for Ardalis.Result in FastEndpoints.
/// Note: do not modfiy methods!
/// </summary>
public static class ResultsExtensions
{
    public static async Task SendResponse<TResult, TResponse>(
        this IEndpoint endpoint,
        TResult result,
        Func<TResult, TResponse> mapper) where TResult : IResult
    {
        switch (result.Status)
        {
            case ResultStatus.Ok:

                await endpoint.HttpContext.Response.SendAsync(mapper(result));

                break;

            case ResultStatus.Invalid:

                var validationFailures = result.ValidationErrors
                    .Select(err => new ValidationFailure(err.Identifier, err.ErrorMessage));

                endpoint.ValidationFailures.AddRange(validationFailures);

                await endpoint.HttpContext.Response.SendErrorsAsync(endpoint.ValidationFailures);

                break;
        }
    }
}
