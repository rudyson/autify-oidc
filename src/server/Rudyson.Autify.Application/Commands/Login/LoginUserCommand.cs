using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Rudyson.Autify.Application.Commands.Login;

public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResult>;
