using MediatR;
using Microservice.Common.CQRS;
using Microservice.Common.Features;
using Microservice.Common.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Microservice.Common.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CRUDController<TModel> : ControllerBase 
    where TModel : class, IIdentity
{
    private readonly IMediator _mediator;

    public CRUDController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public virtual async Task<ActionResult<TModel?>> Create([FromBody] TModel model)
    {
        Guid id = await _mediator.Send(new CreateEntityCommand<TModel>(model));
        return CreatedAtAction(nameof(Read), new { id }, await Read(id));
    }

    [HttpGet]
    public virtual async Task<IEnumerable<TModel>> List([FromQuery] IEnumerable<Guid> ids)
    {
        IEnumerable<TModel>? result;
        if (!ids.Any())
        {
            result = await _mediator.Send(new ListAllEntitiesQuery<TModel>());
        }
        else
        {
            result = await _mediator.Send(new ListEntitiesQuery<TModel>(ids));
        }

        return result ?? Enumerable.Empty<TModel>();
    }

    [HttpGet("{id}")]
    public virtual async Task<TModel?> Read(Guid id)
    {
        var result = await _mediator.Send(new GetEntityQuery<TModel>(id));

        if (result == null)
            Response.StatusCode = (int)HttpStatusCode.NotFound;

        return result;
    }

    [HttpPut("{id}")]
    public virtual async Task<TModel?> Update(Guid id, [FromBody] TModel model)
    {
        model.Id = id;

        await _mediator.Send(new UpdateEntityCommand<TModel>(model));
        return await Read(model.Id);
    }

    [HttpPatch("{id}")]
    public virtual async Task<ActionResult<TModel?>> Patch(Guid id, [FromBody] JsonPatchDocument<TModel> patchDoc)
    {
        if (patchDoc != null && ModelState.IsValid)
        {
            await _mediator.Send(new PatchEntityCommand<TModel>(id, patchDoc));

            return await Read(id);
        }
        else
        {
            return this.BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEntityCommand<TModel>(id));
        return NoContent();
    }
}
