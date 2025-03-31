using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public int PositionId { get; set; }

        [ForeignKey("PositionId")]
        public PositionEmp? Position { get; set; }

        public decimal Rate { get; set; }
    }
}
