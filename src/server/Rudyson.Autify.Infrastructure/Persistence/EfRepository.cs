using Microsoft.EntityFrameworkCore;
using Rudyson.Autify.Domain.Core;
using Rudyson.Autify.Domain.Repositories;

namespace Rudyson.Autify.Infrastructure.Persistence;

public abstract class EfRepository<T, TId> : IRepository<T, TId> where T : AggregateRoot<TId>
{
    protected readonly AutifyContext _context;
    protected EfRepository(AutifyContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(TId id, CancellationToken ct) =>
        await _context.Set<T>().FirstOrDefaultAsync(e => e.Id!.Equals(id), ct);

    public async Task AddAsync(T entity, CancellationToken ct) =>
        await _context.Set<T>().AddAsync(entity, ct);

    public void Update(T entity) => _context.Set<T>().Update(entity);
    public void Delete(T entity) => _context.Set<T>().Remove(entity);
}
