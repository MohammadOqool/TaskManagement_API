using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagementSystemAPI.Models.Context;
using TaskManagementSystemAPI.Models.Entities;
using TaskManagementSystemAPI.RepositoriesLayers.Interfaces;

namespace TaskManagementSystemAPI.RepositoriesLayers.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> currentTable;

        public GenericRepository(AppDBContext appDBContext)
        {
            currentTable = appDBContext.Set<T>();
        }

        public async Task Add(T entity)
        {
            await currentTable.AddAsync(entity);
        }

        public async Task Delete(int Id)
        {
            var record = await Get(Id);
            if (record != null)
            {
                currentTable.Remove(record);
            }
        }

        public async Task<IEnumerable<T>?> GetAll()
        {
            return await currentTable.ToListAsync();
        }

        public async Task<T?> Get(int id)
        {
            return await currentTable.FindAsync(id);
        }

        public async Task<bool> IsExist(int id)
        {
            return await Get(id) != null;
        }

        public async Task Update(T entity, int Id)
        {
            currentTable.Entry(entity).State = EntityState.Modified;
            currentTable.Update(entity);
        }
        public async Task<T?> Find(Expression<Func<T, bool>> predicate)
        {
            return await currentTable.FirstOrDefaultAsync(predicate);
        }
        public async Task<IEnumerable<T>?> FindAll(Expression<Func<T, bool>> predicate)
        {
            return await currentTable.Where(predicate).ToListAsync();
        }
    }
}
