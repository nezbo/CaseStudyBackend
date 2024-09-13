﻿using ErrorOr;
using MediatR;
using Microservice.Common.Domain.Models;

namespace Microservice.Common.Application.Features;

public class DeleteEntityCommand<T>(Guid id) 
    : IRequest<ErrorOr<Deleted>>
    where T : IIdentity
{
    public Guid Id { get; set; } = id;
}
