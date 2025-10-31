using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using TaskManagementSystemAPI.Models;
using TaskManagementSystemAPI.Models.DTO;
using TaskManagementSystemAPI.Models.Entities;
using TaskManagementSystemAPI.RepositoriesLayers.Implementations;
using TaskManagementSystemAPI.RepositoriesLayers.Interfaces;
using TaskManagementSystemAPI.ServicesLayers.Interfaces;
using TaskManagementSystemAPI.UnitOfWorkPattern;

namespace TaskManagementSystemAPI.ServicesLayers.Implementations
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IGenericRepository<T> currentRepository;

        public GenericService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            currentRepository = unitOfWork.GetCurrentRepository<T>();

            //genericRepository = new GenericRepository<T>(unitOfWork.AppDBContext);
        }
        public async Task Add(T entity)
        {
            await currentRepository.Add(entity);
            await unitOfWork.SaveChanges();
        }

        public async Task<bool> Delete(int id)
        {
            var record = await currentRepository.Get(id);
            if (record == null)
                return false;

            await currentRepository.Delete(id);
            await unitOfWork.SaveChanges();

            return true;
        }

        public async Task<ServiceResult<T?>> Get(int id)
        {
            var record = await currentRepository.Get(id);
            if (record == null)
                return ServiceResult<T?>.Failure("No Data Found", HttpStatusCode.NotFound);

            return ServiceResult<T?>.Success(record);
        }

        public async Task<ServiceResult<IEnumerable<T>?>> GetAll()
        {
            var records = await currentRepository.GetAll();

            if (records?.Count() == 0)
                return ServiceResult<IEnumerable<T>?>.Failure(errorMessage: "No Data Found");

            return ServiceResult<IEnumerable<T>?>.Success(records);
        }

        public async Task<bool> IsExist(int id)
        {
            return await currentRepository.IsExist(id);
        }

        public async Task Update(T entity, int Id)
        {
            await currentRepository.Update(entity, Id);
            await unitOfWork.SaveChanges();
        }
        public async Task<ServiceResult<T?>> Find(Expression<Func<T, bool>> predicate)
        {
            var record = await currentRepository.Find(predicate);
            if (record == null)
                return ServiceResult<T?>.Failure("No Data Found", HttpStatusCode.NotFound);

            return ServiceResult<T?>.Success(record);
        }
        public async Task<ServiceResult<IEnumerable<T>?>> FindAll(Expression<Func<T, bool>> predicate)
        {
            var record = await currentRepository.FindAll(predicate);
            if (record == null)
                return ServiceResult<IEnumerable<T>?>.Failure("No Data Found", HttpStatusCode.NotFound);

            return ServiceResult<IEnumerable<T>?>.Success(record);
        }
    }
}
