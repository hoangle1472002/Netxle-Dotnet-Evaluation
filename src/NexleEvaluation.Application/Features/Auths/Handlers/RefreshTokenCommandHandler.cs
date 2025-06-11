using MediatR;
using NexleEvaluation.Application.Features.Auths.Commands;
using NexleEvaluation.Application.Models.Responses;
using NexleEvaluation.Application.Models.Responses.Auth;
using NexleEvaluation.Application.Services.Interfaces.Auths;
using System.Threading;
using System.Threading.Tasks;

namespace NexleEvaluation.Application.Features.Auths.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            return await _authService.RefreshTokenAsync(command.Request);
        }
    }
}