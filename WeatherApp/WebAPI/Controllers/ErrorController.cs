using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Responses;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [HttpGet]
        [Route("/Error")]
        public ErrorResponse GetError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = context?.Error;

            Response.StatusCode = ex switch
            {
                ArgumentException    => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _                    => StatusCodes.Status500InternalServerError
            };

            return new ErrorResponse(ex?.Message ?? "");
        }
    }
}
