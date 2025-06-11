using MediatR;

namespace NexleEvaluation.Application.Features.Auths.Commands
{
    public record LogoutCommand(int UserId) : IRequest<Unit>;
}

