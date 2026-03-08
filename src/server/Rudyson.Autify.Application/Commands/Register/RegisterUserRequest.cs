using TypeGen.Core.TypeAnnotations;

namespace Rudyson.Autify.Application.Commands.Register;

[ExportTsInterface]
public sealed record RegisterUserRequest(
    string Email,
    string Password
);
