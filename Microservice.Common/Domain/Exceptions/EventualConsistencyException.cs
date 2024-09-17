using ErrorOr;

namespace Microservice.Common.Domain.Exceptions;
public class EventualConsistencyException(
    Error eventualConsistencyError,
    List<Error> underlyingErrors) 
    : Exception(message: eventualConsistencyError.Description)
{
    public Error EventualConsistencyError { get; } = eventualConsistencyError;
    public List<Error> UnderlyingErrors { get; } = underlyingErrors ?? [];
}
