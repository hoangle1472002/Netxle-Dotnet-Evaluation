using MediatR;
using NexleEvaluation.Application.Models.Requests.Auths;
using NexleEvaluation.Application.Models.Responses;
using NexleEvaluation.Application.Models.Responses.Auth;

namespace NexleEvaluation.Application.Features.Auths.Commands
{
    public record RefreshTokenCommand(RefreshTokenRequest Request) : IRequest<Result<RefreshTokenResponse>>;
}

