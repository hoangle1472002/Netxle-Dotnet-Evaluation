using System;

namespace NexleEvaluation.Application.Models.Responses.Auth
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiryTime { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
