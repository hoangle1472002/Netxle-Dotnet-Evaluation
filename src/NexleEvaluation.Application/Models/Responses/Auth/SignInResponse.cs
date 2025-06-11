using NexleEvaluation.Application.Models.Dtos.User;

namespace NexleEvaluation.Application.Models.Responses.Auth
{
    public class SignInResponse
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
