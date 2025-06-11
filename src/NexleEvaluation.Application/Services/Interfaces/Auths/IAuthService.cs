using System.Threading.Tasks;
using NexleEvaluation.Application.Models.Requests.Auths;
using NexleEvaluation.Application.Models.Responses;
using NexleEvaluation.Application.Models.Responses.Auth;

namespace NexleEvaluation.Application.Services.Interfaces.Auths
{
    public interface IAuthService
    {
        Task<Result<SignUpResponse>> SignUpAsync(SignUpRequest request);
        Task<Result<SignInResponse>> SignInAsync(SignInRequest request);
        Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task SignOutAsync(int userId);
    }
}
