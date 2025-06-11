using System.ComponentModel.DataAnnotations;

namespace NexleEvaluation.Application.Models.Requests.Auths
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
