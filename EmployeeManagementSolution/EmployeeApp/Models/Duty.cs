using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Models
{
    public class Duty
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public int TaskId { get; set; }
        public string? Name { get; set; }
        public Task? Task { get; set; }
        public Employee? Employee { get; set; }
    }
} 
