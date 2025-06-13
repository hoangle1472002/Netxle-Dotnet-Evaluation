using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexleEvaluation.API.Extensions;
using NexleEvaluation.Application.Features.Auths.Commands;
using NexleEvaluation.Application.Models.Requests.Auths;
using NexleEvaluation.Application.Models.Responses;
using NexleEvaluation.Application.Models.Responses.Auth;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NexleEvaluation.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<Result<SignUpResponse>>> SignUp([FromBody] SignUpRequest request)
        {
            try
            {
                var response = await _mediator.Send(new SignUpCommand(request));

                if (response.Ok)
                {
                    return Created("Created successfully", response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Result<SignUpResponse>
                {
                    Message = "Internal server error: " + ex.Message
                });
            }
        }

        [HttpPost("signin")]
        public async Task<ActionResult<Result<SignInResponse>>> Login([FromBody] SignInRequest request)
        {
            var response = await _mediator.Send(new SignInCommand(request));
            return response.Ok ? Ok(response) : BadRequest(response);
        }

        [HttpPost("refresh-token"), Authorize]
        public async Task<ActionResult<Result<SignInResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _mediator.Send(new RefreshTokenCommand(request));
            return response.Ok ? Ok(response) : BadRequest(response);
        }

        [HttpPost("signout"), Authorize]
        public async Task<ActionResult> SignOut()
        {
            try
            {
                var userId = User.GetUserId();
                await _mediator.Send(new LogoutCommand(userId));
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = "Internal server error: " + ex.Message
                });
            }
        }
    }
}