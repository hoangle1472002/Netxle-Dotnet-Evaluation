using NexleEvaluation.Application.Models.Responses.User;

namespace NexleEvaluation.Application.Models.Responses.Auth
{
    public class SignInResponse
    {
        public UserDetailResponse User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
