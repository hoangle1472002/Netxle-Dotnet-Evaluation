using System.Collections.Generic;
using System.Linq;

namespace NexleEvaluation.Application.Models.Responses
{
    public class Result<TResponse>
    {
        public string Message { get; set; }

        public TResponse Data { get; set; }
        public List<ErrorResponse> Errors { get; set; } = new List<ErrorResponse>();
        public bool Ok => !Errors.Any();
    }
}