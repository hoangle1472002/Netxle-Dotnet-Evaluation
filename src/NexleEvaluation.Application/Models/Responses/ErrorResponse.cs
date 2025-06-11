namespace NexleEvaluation.Application.Models.Responses
{
    public class ErrorResponse
    {
        public string Description { get; set; }

        public static ErrorResponse FromResource(string description)
        {
            return new ErrorResponse()
            {
                Description = description
            };
        }
    }
}
