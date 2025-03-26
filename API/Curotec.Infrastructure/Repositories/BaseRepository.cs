using Curotec.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Curotec.Infrastructure.Repositories
{
    public class BaseRepository<Entity> : IBaseRepository<Entity> where Entity : class
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseRepository(IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
            => (_unitOfWork, _dbContext) = (unitOfWork, dbContext);

        public async Task<Entity> AddAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<Entity>().AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<Entity>> GetByFilterAsync(
            Expression<Func<Entity, bool>> filter,
            Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Entity> query = _dbContext.Set<Entity>();

            if (include != null)
            {
                query = include(query);
            }

            query = query.Where(filter);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Entity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Entity>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<bool> RemoveAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<Entity>().Remove(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Entity> UpdateAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}