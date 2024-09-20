using ErrorOr;
using MediatR;
using Microservice.Common.Application.Features;
using Microservice.Common.Domain.Models;
using Microservice.Common.Extensions;
using Microservice.Common.Presentation.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MoreLinq;
using System.Net;
using System.Reflection;

namespace Microservice.Common.Presentation.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class CRUDController<TModel, TDomain>(IMediator mediator) : ControllerBase
    where TModel : class, IIdentity
    where TDomain : AggregateRoot
{
    protected abstract ErrorOr<TDomain> MapToDomain(TModel model);
    protected abstract TModel MapFromDomain(TDomain model);

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] TModel model)
    {
        var domainModel = this.MapToDomain(model);

        if (domainModel.IsError)
            return this.Problem(domainModel.Errors);

        var response = await mediator.Send(new CreateEntityCommand<TDomain>(domainModel.Value));

        return this.MatchOrProblem(response, obj => CreatedAtAction(nameof(Read), new { obj.Id }, obj));
    }

    [HttpGet]
    public virtual async Task<IActionResult> List([FromQuery] IEnumerable<Guid> ids)
    {
        ErrorOr<IEnumerable<TDomain>> response;
        if (!ids.Any())
        {
            response = await mediator.Send(new ListAllEntitiesQuery<TDomain>());
        }
        else
        {
            response = await mediator.Send(new ListEntitiesQuery<TDomain>(ids));
        }

        return this.MatchOrProblem(response);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Read(Guid id)
    {
        var response = await mediator.Send(new GetEntityQuery<TDomain>(id));

        if (response.IsError)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        
        return this.MatchOrProblem(response);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] TModel model)
    {
        if (id != model.Id)
        {
            ErrorOr.Error error = ErrorOr.Error.Validation(description: "Id in body does not match URL.");
            return this.Problem([error]);
        }

        var domainModel = this.MapToDomain(model);

        if (domainModel.IsError)
            return this.Problem(domainModel.Errors);

        var response = await mediator.Send(new UpdateEntityCommand<TDomain>(id, domainModel.Value));
        return this.MatchOrProblem(response, u => Ok());
    }

    [HttpPatch("{id}")]
    public virtual async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<TModel> patchDoc)
    {
        if (patchDoc != null && ModelState.IsValid)
        {
            var domainModel = await mediator.Send(new GetEntityQuery<TDomain>(id));

            if (domainModel.IsError)
                return Problem();

            var dto = this.MapFromDomain(domainModel.Value);
            patchDoc.ApplyTo(dto);
            domainModel = this.MapToDomain(dto);
            var response = await mediator.Send(new UpdateEntityCommand<TDomain>(id, domainModel.Value));

            return this.MatchOrProblem(response, u => Ok());
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(Guid id)
    {
        var response = await mediator.Send(new DeleteEntityCommand<TDomain>(id));
        return this.MatchOrProblem(response, v => NoContent());
    }

    #region Helpers

    protected ActionResult MatchOrProblem(ErrorOr<TDomain> domain)
    {
        return this.MatchOrProblem(domain, OkFromDomain);
    }

    protected ActionResult OkFromDomain(TDomain domain)
    {
        var dto = this.MapFromDomain(domain);
        if (dto is IHasEditUrl editUrl)
            this.SetEditUrl(editUrl);
        return Ok(dto);
    }

    protected ActionResult MatchOrProblem(ErrorOr<IEnumerable<TDomain>> entities)
    {
        return this.MatchOrProblem(entities, OkFromDomain);
    }

    protected ActionResult OkFromDomain(IEnumerable<TDomain> entities)
    {
        var result = entities.Select(this.MapFromDomain)
            .ForEachThen(this.TrySetEditUrl)
            .ToList();
        return Ok(result);
    }

    protected void TrySetEditUrl<T>(T entity)
    {
        if (entity is IHasEditUrl editUrl)
            this.SetEditUrl(editUrl);
    }

    protected void SetEditUrl(IHasEditUrl entity)
    {
        var controllerName = entity.GetType().Name.TrimEnd("Dto");
        entity.EditUrl = Url.ActionLink(
            controller: controllerName,
            action: nameof(Update),
            values: new { id = entity.Id })!;

        // Nested entities
        GetNestedHasEditUrl(entity).ForEach(SetEditUrl);
    }

    private static IEnumerable<IHasEditUrl> GetNestedHasEditUrl(object parent)
    {
        var direct = parent.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
            .Where(p => p.PropertyType.GetInterfaces().Contains(typeof(IHasEditUrl)))
            .Select(p => p.GetValue(parent) as IHasEditUrl)
            .Where(v => v != null)
            .Select(v => v!);

        var sequences = parent.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
            .Where(p => p.PropertyType.GetInterfaces()
                .Any(i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                    && i.GetGenericArguments().Length == 1
                    && i.GetGenericArguments()[0].GetInterfaces().Contains(typeof(IHasEditUrl))))
            .Select(p => p.GetValue(parent) as IEnumerable<IHasEditUrl>)
            .Where(e => e != null)
            .SelectMany(e => e!);

        return direct.Union(sequences);
    }

    #endregion
}
