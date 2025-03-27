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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Duty>()
                .HasOne(d => d.Task)
                .WithMany()
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Duty>()
                .HasOne(d => d.Employee)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}