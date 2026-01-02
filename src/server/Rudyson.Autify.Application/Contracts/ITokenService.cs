using Rudyson.Autify.Domain.Entities;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Application.Contracts;

public interface ITokenService
{
    string CreateAccessToken(User user);
    (string raw, RefreshToken hash) CreateRefreshToken();
}
