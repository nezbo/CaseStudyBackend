using Microservice.Common.Domain.Models;

namespace Microservice.Common.Presentation.Controllers;

public interface IHasEditUrl : IIdentity
{
    public string? EditUrl { get; set; }
}
