using Microsoft.AspNetCore.Mvc;
using Rudyson.Autify.Application.Contracts;

namespace Rudyson.Autify.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [Obsolete]
    [HttpPost(nameof(Test))]
    public IActionResult Test(IPasswordHasher passwordHasher)
    {
        var password = "password123";
        var hash = passwordHasher.Hash(password);
        var isValid = passwordHasher.Verify(password, hash);

        return isValid ? Ok() : BadRequest();
    }

    [HttpPost(nameof(Register))]
    public Task<IActionResult> Register(CancellationToken cancellationToken = default) // [FromBody] RegisterRequest request, IPasswordHasher passwordHasher
    {
        throw new NotImplementedException();
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
