using System.Net;
using TaskManagementSystemAPI.Models;
using TaskManagementSystemAPI.Models.DTO;
using TaskManagementSystemAPI.Models.Entities;
using TaskManagementSystemAPI.ServicesLayers.Interfaces;
using TaskManagementSystemAPI.UnitOfWorkPattern;

namespace TaskManagementSystemAPI.ServicesLayers.Implementations
{
    public class UserService : GenericService<User>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ServiceResult<UserResponse> PrepareUserResponse(ServiceResult<User> result)
        {
            var castedUserObject = UserResponse.GetResponse(result.Data);

            if (result.IsSuccess)
                return ServiceResult<UserResponse>.Success(castedUserObject);

            return ServiceResult<UserResponse>.Failure(result.ErrorMessage, result.StatusCode);
        }

        public ServiceResult<IEnumerable<UserResponse>> PrepareUserResponse(ServiceResult<IEnumerable<User>?> result)
        {
            var castedUserObject = result.Data?.Select(UserResponse.GetResponse);

            if (result.IsSuccess)
                return ServiceResult<IEnumerable<UserResponse>>.Success(castedUserObject);

            return ServiceResult<IEnumerable<UserResponse>>.Failure(result.ErrorMessage, result.StatusCode);
        }

        public async Task<ServiceResult<UserResponse>> AddNewUser(UserRequest userRequest)
        {
            // check existing user by name
            var existingRecord = await currentRepository.Find(x=> string.Equals(x.Name, userRequest.Name, StringComparison.OrdinalIgnoreCase));
            if (existingRecord != null)
                return ServiceResult<UserResponse>.Failure(errorMessage: "User already exist");

            // prepare enitity then add new record
            var entity = new User() { 
                Name = userRequest.Name,
                Role = (int)userRequest.Role == 1 ? "Admin" : "User"
            };

            await currentRepository.Add(entity);
            await unitOfWork.SaveChanges();

            return ServiceResult<UserResponse>.Success(UserResponse.GetResponse(entity));
        }

        public async Task<ServiceResult<UserResponse>> UpdateUser(int id, UserRequest userRequest, int currentuserId)
        {
            // check existing user
            var existingRecord = await currentRepository.Get(id);
            if (existingRecord == null)
                return ServiceResult<UserResponse>.Failure(errorMessage: "No Data Found");

            if (existingRecord.ID != currentuserId)
                return ServiceResult<UserResponse>.Failure(errorMessage: "Unauthorized Access", HttpStatusCode.Unauthorized);

            // prepare enitity then update new record
            existingRecord.Name = userRequest.Name;
            existingRecord.Role = (int)userRequest.Role == 1 ? "Admin" : "User";

            await currentRepository.Update(existingRecord, id);
            await unitOfWork.SaveChanges();

            return ServiceResult<UserResponse>.Success(UserResponse.GetResponse(existingRecord));
        }

    }
}
