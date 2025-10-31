using System.ComponentModel.DataAnnotations;
using TaskManagementSystemAPI.Models.Entities;

namespace TaskManagementSystemAPI.Models.DTO
{
    public enum UserRole
    {
        Admin = 1, User = 2
    }
    public class UserRequest
    {
        [Required]
        public required string Name { get; set; }

        [Range(1, 2)]
        public required UserRole Role { get; set; }
    }

    public class UserResponse
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }
        public ICollection<UserTaskResponse?>? Tasks { get; set; }

        internal static UserResponse? GetResponse(User? entity)
        {
            if (entity == null)
                return default;

            return new UserResponse()
            {
                ID = entity.ID,
                Name = entity.Name,
                Role = entity.Role,
                Tasks = entity.UserTasks?.Select(UserTaskResponse.GetResponseWithoutRelatedObjects).ToList()
            };
        }

        internal static UserResponse? GetResponseWithoutRelatedObject(User? entity)
        {
            if (entity == null)
                return default;

            return new UserResponse()
            {
                ID = entity.ID,
                Name = entity.Name,
                Role = entity.Role
            };
        }
    }
}
