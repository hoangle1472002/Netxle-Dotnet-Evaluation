using MediatR;
using System.Threading;
using System.Threading.Tasks;
using NexleEvaluation.Application.Features.Auths.Commands;
using NexleEvaluation.Application.Services.Interfaces.Auths;
using NexleEvaluation.Application.Models.Responses;
using NexleEvaluation.Application.Models.Responses.Auth;

namespace NexleEvaluation.Application.Features.Auths.Handlers
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Result<SignUpResponse>>
    {
        private readonly IAuthService _authService;

        public SignUpCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<SignUpResponse>> Handle(SignUpCommand command, CancellationToken cancellationToken)
        {
            return await _authService.SignUpAsync(command.Request);
        }
    }
}

