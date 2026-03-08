using MediatR;
using Rudyson.Autify.Application.Contracts;
using Rudyson.Autify.Domain.Repositories.Inherit;

namespace Rudyson.Autify.Application.Commands.Refresh;

public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResult>;

public record RefreshTokenResult(string AccessToken);

// TODO: Fix bug with building
//[ExportTsInterface]
public record RefreshTokenRequest(string RefreshToken);

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly ISessionRepository _sessions;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ISessionRepository sessions, ITokenService tokenService)
    {
        _sessions = sessions;
        _tokenService = tokenService;
    }

    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        throw new NotImplementedException();
        //// 1. Хешируем полученный токен
        ////var raw, hashed = _tokenService.CreateRefreshToken(request.RefreshToken);

        //// 2. Ищем сессию
        ////var session = await _sessions.GetByRefreshTokenAsync(hashed, ct)
        //    //?? throw new ApplicationException("Invalid refresh token");

        //// 3. Проверка активности (доменная логика)
        //if (!session.IsActive)
        //    throw new ApplicationException("Session expired or revoked");

        //// 4. Генерация нового Access Token
        //var accessToken = _tokenService.CreateAccessToken(session.UserId, session.Id);

        //return new RefreshTokenResult(accessToken);
    }
}
