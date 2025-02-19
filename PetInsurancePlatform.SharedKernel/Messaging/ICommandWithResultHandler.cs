using Ardalis.Result;
using FastEndpoints;

namespace PetInsurancePlatform.SharedKernel.Messaging;

public interface ICommandWithResultHandler<TCommand>
    : ICommandHandler<TCommand, Result> where TCommand
    : ICommandWithResult
{
}

public interface ICommandWithResultHandler<TCommand, TResponse>
    : ICommandHandler<TCommand, Result<TResponse>> where TCommand
    : ICommandWithResult<TResponse>
{
}
