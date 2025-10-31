using System.Linq.Expressions;
using TaskManagementSystemAPI.Models;

namespace TaskManagementSystemAPI.ServicesLayers.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task Add(T entity);
        Task Update(T entity, int Id);
        Task<ServiceResult<T?>> Get(int id);
        Task<ServiceResult<IEnumerable<T>?>> GetAll();
        Task<bool> Delete(int Id);
        Task<bool> IsExist(int id);
        Task<ServiceResult<T?>> Find(Expression<Func<T, bool>> predicate);
        Task<ServiceResult<IEnumerable<T>?>> FindAll(Expression<Func<T, bool>> predicate);
    }
}
