﻿using MediatR;
using Microservice.Common.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Microservice.Common.Features;
public class PatchEntityCommand<T> : IRequest
    where T : class, IIdentity
{
    public Guid Id { get; set; }
    public JsonPatchDocument<T> Patch { get; set; }

    public PatchEntityCommand(Guid id, JsonPatchDocument<T> patch)
    {
        Id = id;
        Patch = patch;
    }
}
