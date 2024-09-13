using ErrorOr;

namespace Microservice.Common.Application.Features.Errors;
public static class CommonErrors
{
    public static readonly Error CreationFailed = Error.Failure("CreationFailed", "Something went wrong when creating the entity.");
    public static readonly Error UpdateFailed = Error.Failure("UpdateFailed", "Something went wrong when updating the entity.");
}
