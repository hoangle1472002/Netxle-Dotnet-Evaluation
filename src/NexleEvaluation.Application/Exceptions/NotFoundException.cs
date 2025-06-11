using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using NexleEvaluation.Application.Models.Responses;

namespace NexleEvaluation.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public int StatusCode { get; } = StatusCodes.Status404NotFound;
        public IEnumerable<ErrorResponse> Errors { get; set; }
        public string ErrorCode { get; set; }

        public NotFoundException()
            : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
