using Rudyson.Autify.Domain.Core;

namespace Rudyson.Autify.Domain.Repositories;

public interface IRepository<T, TId> where T : AggregateRoot<TId>, IAggregateRoot
{
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
}
