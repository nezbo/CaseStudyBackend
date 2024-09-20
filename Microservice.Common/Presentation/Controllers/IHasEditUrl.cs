using Microservice.Common.Domain.Models;

namespace Microservice.Common.Presentation.Controllers;

public interface IHasEditUrl : IGetIdentity
{
    public string? EditUrl { get; set; }
}
