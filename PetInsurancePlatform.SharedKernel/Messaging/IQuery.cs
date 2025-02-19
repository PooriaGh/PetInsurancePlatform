using Ardalis.Result;
using FastEndpoints;

namespace PetInsurancePlatform.SharedKernel.Messaging;

public interface IQuery<TResponse> : ICommand<Result<TResponse>>
{
}
