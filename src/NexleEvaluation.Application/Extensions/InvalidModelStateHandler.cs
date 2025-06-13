using Microsoft.AspNetCore.Mvc;
using NexleEvaluation.Application.Models.Responses;
using System.Linq;

namespace NexleEvaluation.Application.Extensions
{
    public static class InvalidModelStateHandler
    {
        public static IActionResult GenerateErrorResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .SelectMany(kvp =>
                    kvp.Value.Errors.Select(error => new ErrorResponse
                    {
                        Description = error.ErrorMessage
                    })
                )
                .ToList();

            var result = new Result<object>
            {
                Errors = errors,
                Message = "Validation failed."
            };

            return new BadRequestObjectResult(result);
        }
    }
}
