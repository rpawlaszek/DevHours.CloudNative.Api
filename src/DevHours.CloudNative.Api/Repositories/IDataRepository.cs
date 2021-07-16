using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevHours.CloudNative.Repositories
{
    public interface IDataRepository<T>
    {
        IQueryable<T> Query();
        ValueTask<T> GetAsync(int id, CancellationToken token = default);
        Task<T> AddAsync(T room, CancellationToken token = default);
        Task UpdateAsync(T room, CancellationToken token = default);
        Task DeleteAsync(T item, CancellationToken token = default);
    }
}