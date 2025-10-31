using System.Net;
using TaskManagementSystemAPI.Models;
using TaskManagementSystemAPI.Models.DTO;
using TaskManagementSystemAPI.Models.Entities;
using TaskManagementSystemAPI.ServicesLayers.Interfaces;
using TaskManagementSystemAPI.UnitOfWorkPattern;

namespace TaskManagementSystemAPI.ServicesLayers.Implementations
{
    public class UserTaskService : GenericService<UserTask>, IUserTaskService
    {
        private readonly IUserService userService;

        public UserTaskService(IUnitOfWork unitOfWork, IUserService userService) : base(unitOfWork)
        {
            this.userService = userService;
        }

        public async Task<ServiceResult<UserTaskResponse>> AssignedTaskToUser(UserTaskRequest userTaskRequest)
        {
            // check exiting user
            var existingUserResult = await userService.Get(userTaskRequest.UserId);
            if (!existingUserResult.IsSuccess)
                return ServiceResult<UserTaskResponse>.Failure(existingUserResult.ErrorMessage);

            // create new task then assign tp the user
            existingUserResult.Data?.UserTasks?.Add(new UserTask()
            {
                Details = userTaskRequest.Details,
                UserId = userTaskRequest.UserId,
            });

            await userService.Update(existingUserResult.Data, userTaskRequest.UserId);
            await unitOfWork.SaveChanges();

            // prepare the respone for the task
            var userTaskResponse = await Get(existingUserResult.Data.UserTasks.LastOrDefault().ID);
            return ServiceResult<UserTaskResponse>.Success(UserTaskResponse.GetResponse(userTaskResponse.Data));
        }

        public ServiceResult<UserTaskResponse> PrepareResponse(ServiceResult<UserTask?> result)
        {
            var castedObject = UserTaskResponse.GetResponse(result.Data);

            if (result.IsSuccess)
                return ServiceResult<UserTaskResponse>.Success(castedObject);

            return ServiceResult<UserTaskResponse>.Failure(result.ErrorMessage, result.StatusCode);
        }

        public ServiceResult<IEnumerable<UserTaskResponse>> PrepareResponse(ServiceResult<IEnumerable<UserTask>?> result)
        {
            var castedObject = result.Data?.Select(UserTaskResponse.GetResponse);

            if (result.IsSuccess)
                return ServiceResult<IEnumerable<UserTaskResponse>>.Success(castedObject);

            return ServiceResult<IEnumerable<UserTaskResponse>>.Failure(result.ErrorMessage, result.StatusCode);
        }

        public async Task<ServiceResult<UserTaskResponse>> UpdateTask(int id, UpdateUserTaskRequest updateUserTaskRequest, int currentUserId)
        {
            // check existing task
            var existingRecord = await currentRepository.Get(id);
            if (existingRecord == null)
                return ServiceResult<UserTaskResponse>.Failure(errorMessage: "No Data Found");

            var userRole = "";

            // check existing user session
            var existingUserRecord = await userService.Get(currentUserId);
            if (existingUserRecord == null)
                return ServiceResult<UserTaskResponse>.Failure(errorMessage: "No User Found");

            // check user permission if admin or user
            userRole = existingUserRecord.Data?.Role;

            // normal user but the task is not his/her task
            if (existingRecord.User?.ID != currentUserId && string.Equals(userRole, "User", StringComparison.OrdinalIgnoreCase))
                return ServiceResult<UserTaskResponse>.Failure(errorMessage: "Unauthorized Access", HttpStatusCode.Unauthorized);
            
            // update based on the role
            // prepare enitity then update new record
            if (string.Equals(userRole, "User", StringComparison.OrdinalIgnoreCase))
            {
                existingRecord.Status = updateUserTaskRequest.Status ?? existingRecord.Status;
            }
            else
            {
                // admin role
                existingRecord.Details = updateUserTaskRequest.Details ?? existingRecord.Details;
                existingRecord.UserId = updateUserTaskRequest.UserId ?? existingRecord.UserId;
                existingRecord.Status = updateUserTaskRequest.Status ?? existingRecord.Status;
            }

            await Update(existingRecord, id);
            return ServiceResult<UserTaskResponse>.Success(UserTaskResponse.GetResponse(existingRecord));
        }
    }
}
