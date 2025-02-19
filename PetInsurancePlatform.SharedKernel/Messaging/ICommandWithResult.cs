using Ardalis.Result;
using FastEndpoints;

namespace PetInsurancePlatform.SharedKernel.Messaging;

public interface ICommandWithResult : ICommand<Result>
{
}

public interface ICommandWithResult<TResponse> : ICommand<Result<TResponse>>
{
}
