using System.ComponentModel.DataAnnotations;
using TaskManagementSystemAPI.Models.Entities;

namespace TaskManagementSystemAPI.Models.DTO
{
    public class UserTaskRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public required string Details { get; set; }
    }

    public class UpdateUserTaskRequest
    {
        public int? UserId { get; set; }
        public string? Details { get; set; }
        public string? Status { get; set; }
    }

    public class UserTaskResponse
    {
        public required int ID { get; set; }
        public required string Details { get; set; }
        public required string Status { get; set; }
        public UserResponse? User { get; set; }

        public static UserTaskResponse? GetResponse(UserTask? userTask)
        {
            if (userTask == null)
                return default;

            return new UserTaskResponse()
            {
                ID = userTask.ID,
                Details = userTask.Details,
                Status = userTask.Status,
                User = UserResponse.GetResponseWithoutRelatedObject(userTask.User),
            };
        }
        public static UserTaskResponse? GetResponseWithoutRelatedObjects(UserTask? userTask)
        {
            if (userTask == null)
                return default;

            return new UserTaskResponse()
            {
                ID = userTask.ID,
                Details = userTask.Details,
                Status = userTask.Status
            };
        }
    }
}
