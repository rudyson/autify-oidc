using System;
using System.Collections.Generic;
using System.Text;
using Rudyson.Autify.Domain.Entities;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken ct);
    Task<User?> GetByIdAsync(UserId id, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
}
