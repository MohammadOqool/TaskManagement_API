using TaskManagementSystemAPI.Models;
using TaskManagementSystemAPI.Models.DTO;
using TaskManagementSystemAPI.Models.Entities;

namespace TaskManagementSystemAPI.ServicesLayers.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task<ServiceResult<UserResponse>> AddNewUser(UserRequest userRequest);
        Task<ServiceResult<UserResponse>> UpdateUser(int id, UserRequest userRequest, int currentuserId);
        ServiceResult<UserResponse> PrepareUserResponse(ServiceResult<User> result);
        ServiceResult<IEnumerable<UserResponse>> PrepareUserResponse(ServiceResult<IEnumerable<User>?>? result);
    }
}
