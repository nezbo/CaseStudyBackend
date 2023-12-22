using MediatR;
using Microservice.Common.CQRS;
using Microservice.Common.Features;
using Microservice.Common.Models;
using Microservice.Common.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Microservice.Common.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CRUDController<TModel>(IMediator mediator) : ControllerBase 
    where TModel : class, IIdentity
{
    [HttpPost]
    public virtual async Task<ActionResult<TModel?>> Create([FromBody] TModel model)
    {
        Guid id = await mediator.Send(new CreateEntityCommand<TModel>(model));
        return CreatedAtAction(nameof(Read), new { id }, await Read(id));
    }

    [HttpGet]
    public virtual async Task<IEnumerable<TModel>> List([FromQuery] IEnumerable<Guid> ids)
    {
        IEnumerable<TModel>? result;
        if (!ids.Any())
        {
            result = await mediator.Send(new ListAllEntitiesQuery<TModel>());
        }
        else
        {
            result = await mediator.Send(new ListEntitiesQuery<TModel>(ids));
        }

        return (result ?? Enumerable.Empty<TModel>())
            .ForEach(TrySetEditUrl);
    }

    [HttpGet("{id}")]
    public virtual async Task<TModel?> Read(Guid id)
    {
        var result = await mediator.Send(new GetEntityQuery<TModel>(id));

        if (result == null)
            Response.StatusCode = (int)HttpStatusCode.NotFound;

        TrySetEditUrl(result!);

        return result;
    }

    [HttpPut("{id}")]
    public virtual async Task<TModel?> Update(Guid id, [FromBody] TModel model)
    {
        model.Id = id;

        await mediator.Send(new UpdateEntityCommand<TModel>(model));
        return await Read(model.Id);
    }

    [HttpPatch("{id}")]
    public virtual async Task<ActionResult<TModel?>> Patch(Guid id, [FromBody] JsonPatchDocument<TModel> patchDoc)
    {
        if (patchDoc != null && ModelState.IsValid)
        {
            await mediator.Send(new PatchEntityCommand<TModel>(id, patchDoc));

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
        await mediator.Send(new DeleteEntityCommand<TModel>(id));
        return NoContent();
    }

    protected void TrySetEditUrl(IIdentity result)
    {
        if (result is IHasEditUrl editUrl)
        {
            editUrl.EditUrl = Url.ActionLink(action: nameof(Update), values: new { id = result.Id })!;
        }
    }
}
