using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NexleEvaluation.Application.Exceptions;
using NexleEvaluation.Application.Models.Responses;

namespace NexleEvaluation.API.Extensions.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var result = new Result<string>();
            switch (ex)
            {
                case NotFoundException notFoundException:
                    response.StatusCode = notFoundException.StatusCode;
                    result.Errors = notFoundException.Errors.ToList();
                    result.Message = notFoundException.Message;
                    break;

                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    _logger.LogError(ex, "Unhandled exception occurred while executing request: {Method} {Url}, UserId: {UserId}",
                                     context.Request.Method,
                                     context.Request.Path,
                                     context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value); 
                    break;
            }
            await response.WriteAsJsonAsync(result);
        }
    }
}
