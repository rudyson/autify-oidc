using TypeGen.Core.TypeAnnotations;

namespace Rudyson.Autify.Application.Commands.Login;

[ExportTsInterface]
public record LoginUserRequest(string Email, string Password);
