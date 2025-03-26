using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Curotec.Infrastructure.Repositories
{
    public interface IBaseRepository<Entity> where Entity : class
    {
        Task<Entity> AddAsync(Entity entity, CancellationToken cancellationToken = default);
        Task<Entity> UpdateAsync(Entity entity, CancellationToken cancellationToken = default);
        Task<bool> RemoveAsync(Entity entity, CancellationToken cancellationToken = default);
        Task<Entity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Entity>> GetByFilterAsync(Expression<Func<Entity, bool>> filter, Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include, CancellationToken cancellationToken = default);
    }
} 