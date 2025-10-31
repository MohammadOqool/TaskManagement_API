using TaskManagementSystemAPI.Models.Context;
using TaskManagementSystemAPI.Models.Entities;
using TaskManagementSystemAPI.RepositoriesLayers.Interfaces;

namespace TaskManagementSystemAPI.UnitOfWorkPattern
{
    public interface IUnitOfWork
    {
        public AppDBContext AppDBContext { get; }
        public IGenericRepository<User> UserRepository { get; }
        public IGenericRepository<UserTask> UserTaskRepository { get; }

        IGenericRepository<T> GetCurrentRepository<T>() where T : class;

        public Task SaveChanges();
    }
}
