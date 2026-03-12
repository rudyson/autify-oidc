using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rudyson.Autify.Infrastructure.Persistence;

namespace Rudyson.Autify.Server.Controllers;

// TODO: Move business logic to a service and inject it into the controller. This will make the code more testable and maintainable.
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _applicationDbContext;
    private static bool _databaseChecked;

    public AccountController(
          SignInManager<IdentityUser> signInManager,
          UserManager<IdentityUser> userManager,
          ApplicationDbContext applicationDbContext)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _applicationDbContext = applicationDbContext;
    }

    //
    // POST: /Account/Register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        EnsureDatabaseCreated(_applicationDbContext);
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            AddErrors(result);
        }

        // If we got this far, something failed.
        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return Unauthorized();

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized();

        await _signInManager.SignInAsync(user, model.RememberMe);

        return Ok();
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    #region Helpers

    // The following code creates the database and schema if they don't exist.
    // This is a temporary workaround since deploying database through EF migrations is
    // not yet supported in this release.
    // Please see this http://go.microsoft.com/fwlink/?LinkID=615859 for more information on how to do deploy the database
    // when publishing your application.
    private static void EnsureDatabaseCreated(ApplicationDbContext context)
    {
        if (!_databaseChecked)
        {
            _databaseChecked = true;
            context.Database.EnsureCreated();
        }
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    #endregion
}
