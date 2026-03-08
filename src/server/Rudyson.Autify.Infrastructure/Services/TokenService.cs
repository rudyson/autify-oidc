using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rudyson.Autify.Domain.ValueObjects;
using Rudyson.Autify.Infrastructure.Options;

namespace Rudyson.Autify.Infrastructure.Services;

public class TokenService
{
    private readonly JwtOptions _jwtOptions;
    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string CreateAccessToken(UserId userId, SessionId sessionId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Sid, sessionId.Value.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRawRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var raw = Convert.ToBase64String(bytes);

        return raw;
    }

    public static RefreshTokenHash HashRefreshToken(string raw)
    {
        var bytes = Encoding.UTF8.GetBytes(raw);
        var hash = SHA256.HashData(bytes);

        return RefreshTokenHash.FromHash(
            Convert.ToBase64String(hash)
        );
    }

    public (string Raw, RefreshTokenHash Hashed) CreateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var raw = Convert.ToBase64String(bytes);

        var hashed = HashRefreshToken(raw);

        return (raw, hashed);
    }

    // TODO: Remove to MediatR handler

    //public async Task<string> Handle(
    //RefreshCommand command,
    //CancellationToken ct)
    //{
    //    // 1. Хешируем то, что прислал пользователь
    //    var hashed = _tokenService.HashRefreshToken(command.RefreshToken);

    //    // 2. Ищем сессию
    //    var session = await _sessions.GetByRefreshTokenAsync(hashed, ct)
    //        ?? throw new ApplicationException("Invalid refresh token");

    //    // 3. Проверяем доменные инварианты
    //    if (!session.IsActive)
    //        throw new ApplicationException("Session expired or revoked");

    //    // 4. Выдаём новый access token
    //    return _tokenService.CreateAccessToken(
    //        session.UserId,
    //        session.Id
    //    );
    //}
}
