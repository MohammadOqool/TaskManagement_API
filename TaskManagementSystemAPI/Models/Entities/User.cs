using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystemAPI.Models.Entities
{
    [Table("USER")]
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Column("NAME")]
        public required string Name { get; set; }

        [Column("ROLE")]
        public required string Role { get; set; }

        public virtual ICollection<UserTask>? UserTasks { get; set; }
    }
}
