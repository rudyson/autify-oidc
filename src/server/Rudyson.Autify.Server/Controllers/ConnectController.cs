using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Rudyson.Autify.Infrastructure.Persistence;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Rudyson.Autify.Server.Controllers;

[ApiController]
public class ConnectController : Controller
{
    private readonly IOpenIddictApplicationManager _applicationManager;

    public ConnectController(IOpenIddictApplicationManager applicationManager)
        => _applicationManager = applicationManager;

    [HttpPost("~/connect/token"), Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        if (request.IsAuthorizationCodeGrantType())
        {
            var result = await HttpContext.AuthenticateAsync(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var principal = result.Principal!;

            return SignIn(principal,
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (request.IsClientCredentialsGrantType())
        {
            // Note: the client credentials are automatically validated by OpenIddict:
            // if client_id or client_secret are invalid, this action won't be invoked.

            var application = await _applicationManager.FindByClientIdAsync(request.ClientId) ??
                throw new InvalidOperationException("The application cannot be found.");

            // Create a new ClaimsIdentity containing the claims that
            // will be used to create an id_token, a token or a code.
            var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);

            // Use the client_id as the subject identifier.
            identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
            identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));

            identity.SetDestinations(static claim => claim.Type switch
            {
                // Allow the "name" claim to be stored in both the access and identity tokens
                // when the "profile" scope was granted (by calling principal.SetScopes(...)).
                Claims.Name when claim.Subject!.HasScope(Scopes.Profile)
                    => [Destinations.AccessToken, Destinations.IdentityToken],

                // Otherwise, only store the claim in the access tokens.
                _ => [Destinations.AccessToken]
            });

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new NotImplementedException("The specified grant is not implemented.");
    }

    [HttpGet("~/connect/authorize")]
    public IActionResult Authorize()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return Challenge(IdentityConstants.ApplicationScheme);
        }

        var identity = new ClaimsIdentity(
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        identity.AddClaim(OpenIddictConstants.Claims.Subject,
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        identity.AddClaim(OpenIddictConstants.Claims.Email,
            User.FindFirstValue(ClaimTypes.Email)!);

        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(new[]
        {
        OpenIddictConstants.Scopes.OpenId,
        OpenIddictConstants.Scopes.Email,
        OpenIddictConstants.Scopes.Profile
    });

        return SignIn(principal,
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpGet("~/connect/userinfo")]
    public IActionResult UserInfo()
    {
        return Ok(new
        {
            sub = User.FindFirstValue(ClaimTypes.NameIdentifier),
            email = User.FindFirstValue(ClaimTypes.Email),
            name = User.Identity!.Name
        });
    }
}
