using Microsoft.EntityFrameworkCore;
using Rudyson.Autify.Domain.Entities;
using Rudyson.Autify.Domain.Repositories.Inherit;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Infrastructure.Persistence;

public class UserRepository : EfRepository<User, UserId>, IUserRepository
{
    public UserRepository(AutifyContext context) : base(context) { }

    public Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

}
