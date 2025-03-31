using Microsoft.EntityFrameworkCore;
using EmployeeApp.Models;
using Task = EmployeeApp.Models.Task;

namespace EmployeeApp.Data
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PositionEmp> Positions { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Duty> Duties { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "Server=localhost;Database=EmployeeDB;User=root;Password=281105;",
                ServerVersion.AutoDetect("Server=localhost;Database=employeedb;User=root;Password=281105;"));
        }
    }
}
