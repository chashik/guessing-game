using GuessingGame.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuessingGame.Data
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Table { get; }

        Task Insert(TEntity entity, CancellationToken cancellationToken = default);

        Task Update(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> GetById(object id, CancellationToken cancellationToken = default);
    }
}
