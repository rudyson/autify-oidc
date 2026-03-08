using TypeGen.Core.TypeAnnotations;

namespace Rudyson.Autify.Application.Commands.Login;

// TODO: Fix bug with building
//[ExportTsInterface]
public record LoginUserRequest(string Email, string Password);
