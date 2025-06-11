using NexleEvaluation.Application.Models.Responses.Auth;
using NexleEvaluation.Domain.Entities.Identity;

namespace NexleEvaluation.Application.Services.Interfaces.Auths
{
    public interface IJwtTokenManager
    {
        TokenResponse GenerateClaimsToken(User user);
    }
}
