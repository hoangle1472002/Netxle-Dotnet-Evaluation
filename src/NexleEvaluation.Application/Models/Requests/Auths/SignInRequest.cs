using System.ComponentModel.DataAnnotations;

namespace NexleEvaluation.Application.Models.Requests.Auths
{
    public class SignInRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8), MaxLength(20)]
        public string Password { get; set; }
    }
}
