using TaskManagementSystemAPI.Models.Context;
using TaskManagementSystemAPI.Models.DTO;
using TaskManagementSystemAPI.Models.Entities;
using TaskManagementSystemAPI.RepositoriesLayers.Implementations;
using TaskManagementSystemAPI.RepositoriesLayers.Interfaces;

namespace TaskManagementSystemAPI.UnitOfWorkPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories;

        public UnitOfWork(AppDBContext appDBContext)
        {
            UserRepository = new GenericRepository<User>(appDBContext);
            UserTaskRepository = new GenericRepository<UserTask>(appDBContext);
            AppDBContext = appDBContext;

            _repositories = new Dictionary<string, object>()
            {
                { nameof(User), UserRepository },
                { nameof(UserTask), UserTaskRepository },
            };
        }
        public AppDBContext AppDBContext { private set; get; }

        public IGenericRepository<User> UserRepository { get; private set; }

        public IGenericRepository<UserTask> UserTaskRepository { get; private set; }

        public IGenericRepository<T> GetCurrentRepository<T>() where T : class
        {
            var entityType = typeof(T).Name;
            if (!_repositories.TryGetValue(entityType, out object? value))
            {
                throw new NotImplementedException("Failed to implement the repository");
            }
            return (IGenericRepository<T>)value;
        }

        public async Task SaveChanges()
        {
            await AppDBContext.SaveChangesAsync();
        }

    }
}
