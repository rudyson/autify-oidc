using System;
using System.Collections.Generic;
using System.Text;
using Rudyson.Autify.Domain.Repositories;
using Rudyson.Autify.Domain.Repositories.Inherit;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Infrastructure.Services;

public class IdentityService
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IdentityService(
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    // TODO: End method
    public async Task<string> LoginAsync(Email email, string password, CancellationToken ct)
    {
        // 1. Поиск пользователя
        var user = await _userRepository.GetByEmailAsync(email, ct)
            ?? throw new Exception("Invalid credentials");

        // 2. Логика проверки пароля (здесь упрощено)
        // ...

        // 3. Создание сессии через доменный метод User (DDD подход)
        var resfreshToken = string.Empty;
        //var resfreshToken = RefreshTokenHash.GenerateNew();
        var refreshTokenHash = RefreshTokenHash.FromHash(resfreshToken);
        var session = user.CreateSession(refreshTokenHash, DateTime.UtcNow.AddDays(7));

        // 4. Сохранение
        await _sessionRepository.AddAsync(session, ct);

        // 5. Фиксация транзакции
        await _unitOfWork.SaveChangesAsync(ct);

        return resfreshToken;
    }

    public async Task RevokeSessionAsync(RefreshTokenHash token, CancellationToken ct)
    {
        var session = await _sessionRepository.GetByRefreshTokenAsync(token, ct);
        if (session == null) return;

        session.Revoke(); // Доменная логика внутри сущности

        _sessionRepository.Update(session); // Помечаем как измененный
        await _unitOfWork.SaveChangesAsync(ct); // Коммит
    }
}
