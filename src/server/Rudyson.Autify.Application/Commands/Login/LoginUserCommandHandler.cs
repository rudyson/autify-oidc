using MediatR;
using Rudyson.Autify.Application.Contracts;
using Rudyson.Autify.Domain.Repositories;
using Rudyson.Autify.Domain.Repositories.Inherit;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Application.Commands.Login;

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
{
    private readonly IUserRepository _users;
    private readonly ISessionRepository _sessions;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _uow;

    public LoginUserCommandHandler(
        IUserRepository users,
        ISessionRepository sessions,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork uow)
    {
        _users = users;
        _sessions = sessions;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _uow = uow;
    }

    public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken = default)
    {
        var email = new Email(request.Email);
        var user = await _users.GetByEmailAsync(email, cancellationToken)
            ?? throw new ApplicationException("Invalid credentials");

        
         if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new ApplicationException("Invalid credentials");

        var (rawRefreshToken, hashedToken) = _tokenService.CreateRefreshToken();

        var session = user.CreateSession(hashedToken, DateTime.UtcNow.AddDays(7));

        await _sessions.AddAsync(session, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var accessToken = _tokenService.CreateAccessToken(user);

        return new LoginUserResult(accessToken, rawRefreshToken);
    }
}
