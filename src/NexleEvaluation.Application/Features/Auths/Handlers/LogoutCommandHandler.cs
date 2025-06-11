using MediatR;
using System.Threading;
using System.Threading.Tasks;
using NexleEvaluation.Application.Features.Auths.Commands;
using NexleEvaluation.Application.Services.Interfaces.Auths;

namespace NexleEvaluation.Application.Features.Auths.Handlers
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IAuthService _authService;

        public LogoutCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Unit> Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            await _authService.SignOutAsync(command.UserId);
            return Unit.Value;
        }
    }
}

