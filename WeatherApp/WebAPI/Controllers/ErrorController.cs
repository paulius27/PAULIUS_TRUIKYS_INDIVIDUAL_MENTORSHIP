using BL.Validation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [HttpGet]
        [Route("/Error")]
        public IActionResult GetError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = context?.Error;

            if (ex is ValidationException)
            {
                var errors = new ModelStateDictionary();
                errors.AddModelError(((ValidationException)ex).ParamName, ex.Message);
                return ValidationProblem(errors);
            }
            else if (ex is KeyNotFoundException)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }

            return Problem(ex?.Message);
        }
    }
}
