using FluentValidation;
using Microservice.Common.Application.Features;

namespace Microservice.Common.Application.Features.Validation;

public class ListEntitiesValidator<TEntity> : AbstractValidator<ListEntitiesQuery<TEntity>>
{
    public ListEntitiesValidator()
    {
        RuleFor(x => x.Ids).NotEmpty()
            .WithErrorCode("NotEmpty")
            .WithMessage("Provide at least one Asset id to retrieve.");
    }
}