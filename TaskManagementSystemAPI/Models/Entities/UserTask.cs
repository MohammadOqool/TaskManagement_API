using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystemAPI.Models.Entities
{
    [Table("USER_TASK")]
    public class UserTask
    {
        [Key]
        public int ID { get; set; }

        [Column("DETAILS")]
        public required string Details { get; set; }

        [Column("STATUS")]
        public string Status { get; set; } = "Pending";

        [Column("USER_ID")]
        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}
