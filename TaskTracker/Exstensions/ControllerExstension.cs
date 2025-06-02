using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskTracker.Common;

namespace TaskTracker.Exstensions
{
    public static class ControllerExstension
    {
        public static BadRequestObjectResult ValidationError(this ControllerBase controller, ModelStateDictionary modelState, string? message = null)
        {
            var response = ApiResponse<Dictionary<string, string[]>>.BadRequest(modelState, message);
            return new BadRequestObjectResult(response);
        }
    }
}
