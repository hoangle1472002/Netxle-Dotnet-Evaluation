using System.ComponentModel.DataAnnotations;

namespace NexleEvaluation.Application.Models.Requests.Auths
{
    public class SignUpRequest
    {
        [EmailAddress, Required]
        public string Email { get; set; }

        [Required, MinLength(8), MaxLength(20)]
        public string Password { get; set; }

        [MaxLength(32)]
        public string FirstName { get; set; }

        [MaxLength(32)]
        public string LastName { get; set; }
    }
}