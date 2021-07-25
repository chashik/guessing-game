using GuessingGame.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuessingGame.Data
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _dbContext;

        private readonly DbSet<TEntity> _entities;

        // did not apply additional abstarctions here due to time limitations
        public EFRepository(GGDbContext dbContext)
        {
            _dbContext = dbContext;

            _entities = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Table => _entities;

        public async Task Insert(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntity> GetById(
            object id, 
            CancellationToken cancellationToken = default) =>
            await _dbContext.FindAsync<TEntity>(id, cancellationToken);

        public async Task Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
