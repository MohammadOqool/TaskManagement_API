using TaskManagementSystemAPI.Models;
using TaskManagementSystemAPI.Models.DTO;
using TaskManagementSystemAPI.Models.Entities;

namespace TaskManagementSystemAPI.ServicesLayers.Interfaces
{
    public interface IUserTaskService : IGenericService<UserTask>
    {
        Task<ServiceResult<UserTaskResponse>> AssignedTaskToUser(UserTaskRequest userTaskRequest);
        ServiceResult<UserTaskResponse> PrepareResponse(ServiceResult<UserTask?> result);
        ServiceResult<IEnumerable<UserTaskResponse>> PrepareResponse(ServiceResult<IEnumerable<UserTask>?>? result);
        Task<ServiceResult<UserTaskResponse>> UpdateTask(int id, UpdateUserTaskRequest userTaskRequest, int currentUserId);
    }
}
