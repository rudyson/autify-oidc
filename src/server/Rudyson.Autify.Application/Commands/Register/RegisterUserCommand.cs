using MediatR;

namespace Rudyson.Autify.Application.Commands.Register;

public sealed record RegisterUserCommand(
    string Email,
    string Password
) : IRequest<RegisterUserResult>;
