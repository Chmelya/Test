using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AM.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }

    protected IActionResult Problem(Error error)
    {
        var statusCode = error.NumericType switch
        {
            (int)ErrorType.Validation => StatusCodes.Status400BadRequest,
            (int)ErrorType.Conflict => StatusCodes.Status409Conflict,
            (int)ErrorType.NotFound => StatusCodes.Status404NotFound,
            (int)ErrorType.Unauthorized => StatusCodes.Status404NotFound,
            (int)ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            (int)ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            (int)ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(
            statusCode: statusCode,
            detail: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (Error error in errors)
        {
            modelStateDictionary.AddModelError(
                error.Code,
                error.Description);
        }

        return ValidationProblem(modelStateDictionary);
    }
}
