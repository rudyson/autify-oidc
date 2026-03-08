namespace Rudyson.Autify.Application.Commands.Register;

// TODO: Fix bug with building
//[ExportTsInterface]
public sealed record RegisterUserRequest(
    string Email,
    string Password
);
