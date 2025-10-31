using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace TaskManagementSystemAPI.RepositoriesLayers.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task Add(T entity);
        Task Update(T entity, int Id);
        Task<T?> Get(int id);
        Task<IEnumerable<T>?> GetAll();
        Task Delete(int Id);
        Task<bool> IsExist(int id);
        Task<T?> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>?> FindAll(Expression<Func<T, bool>> predicate);
    }
}
