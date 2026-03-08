using MediatR;
using Rudyson.Autify.Application.Contracts;
using Rudyson.Autify.Domain.Entities;
using Rudyson.Autify.Domain.Repositories;
using Rudyson.Autify.Domain.Repositories.Inherit;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Application.Commands.Register;

public sealed class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _uow;

    public RegisterUserCommandHandler(
        IUserRepository users,
        IPasswordHasher passwordHasher,
        IUnitOfWork uow)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _uow = uow;
    }

    public async Task<RegisterUserResult> Handle(
        RegisterUserCommand request,
        CancellationToken ct)
    {
        var email = new Email(request.Email);

        var exists = await _users.ExistsByEmailAsync(email, ct);
        if (exists)
            throw new ApplicationException("User already exists");

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = User.Register(email, passwordHash);

        await _users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return new RegisterUserResult(user.Id.Value);
    }
}
