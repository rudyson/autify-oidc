using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rudyson.Autify.Application.Commands.Register;

namespace Rudyson.Autify.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RegisterUserCommand(
           request.Email,
           request.Password
       );

        var result = await _mediator.Send(command, cancellationToken);

        return Created(
            $"users/{result.UserId}",
            result
        );
    }

    [HttpPost(nameof(Login))]
    public Task<IActionResult> Login(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [HttpPost(nameof(Logout))]
    public Task<IActionResult> Logout(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
