using MediatR;
using System.Threading;
using System.Threading.Tasks;
using NexleEvaluation.Application.Features.Auths.Commands;
using NexleEvaluation.Application.Services.Interfaces.Auths;
using NexleEvaluation.Application.Models.Responses;
using NexleEvaluation.Application.Models.Responses.Auth;

namespace NexleEvaluation.Application.Features.Auths.Handlers
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, Result<SignInResponse>>
    {
        private readonly IAuthService _authService;

        public SignInCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<SignInResponse>> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            return await _authService.SignInAsync(command.Request);
        }
    }
}

