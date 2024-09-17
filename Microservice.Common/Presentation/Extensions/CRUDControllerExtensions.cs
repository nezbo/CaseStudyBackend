using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Microservice.Common.Presentation.Extensions;
public static class CRUDControllerExtensions
{
    #region ProblemDetails

    #endregion

    public static ActionResult MatchOrProblem<TValue>(
        this ControllerBase controller,
        ErrorOr<TValue> source,
        Func<TValue, ActionResult> onValue)
    {
        return source.Match(onValue, e => Problem(controller, e));
    }

    public static ObjectResult Problem(this ControllerBase controller, List<Error> errors)
    {
        int statusCode = 400;
        var problem = controller.ProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext,
            statusCode,
            "One or more errors occurred.",
           type: $"https://httpstatuses.com/{statusCode}",
           detail: "Please refer to the 'errors' property for additional details.",
           instance: controller.HttpContext.Request.Path);

        problem.Extensions.Add("errors", errors);

        var result = new ObjectResult(problem) { StatusCode = statusCode };
        result.ContentTypes.Add(MediaTypeNames.Application.ProblemJson);

        return result;
    }
}
