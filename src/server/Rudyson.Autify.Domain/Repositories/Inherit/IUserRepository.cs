using Rudyson.Autify.Domain.Entities;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Domain.Repositories.Inherit;

public interface IUserRepository : IRepository<User, UserId>
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default);
}
