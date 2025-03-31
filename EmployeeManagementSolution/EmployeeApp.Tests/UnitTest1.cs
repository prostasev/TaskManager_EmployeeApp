using EmployeeApp.Data;
using EmployeeApp.Models;
using EmployeeApp.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Task = EmployeeApp.Models.Task;

namespace EmployeeApp.Tests
{
    [TestFixture]
    public class AuthServiceTests
    {
        private EmployeeDbContext _context;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            // Настройка In-Memory базы данных для каждого теста
            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new EmployeeDbContext(options);
            _authService = new AuthService();

            // Очистка базы перед каждым тестом
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Добавление тестовых данных
            SeedTestData();
        }

        private void SeedTestData()
        {
            var position = new PositionEmp { Id = 1, PositionName = "Developer", Position = "Administrator" };
            var employee = new Employee
            {
                Id = 1,
                FullName = "Иван Иванов",
                Username = "ivan",
                PasswordHash = "12345",
                PositionId = 1,
                Rate = 10.5m
            };
            var task = new Task { Id = 1, Name = "Task1", Description = "Test task" };

            _context.Positions.Add(position);
            _context.Employees.Add(employee);
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        // Тест 1: Проверка аутентификации
        [Test]
        public void Authenticate_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = "admin";
            string password = "hashedpassword";

            // Act
            bool result = _authService.Authenticate(username, password);

            // Assert
            Assert.IsTrue(result, "Аутентификация должна пройти успешно с правильными данными.");
        }

        [Test]
        public void Authenticate_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string username = "ivan";
            string password = "wrongpassword";

            // Act
            bool result = _authService.Authenticate(username, password);

            // Assert
            Assert.IsFalse(result, "Аутентификация должна завершиться неудачей с неверным паролем.");
        }

        // Тест 2: Проверка прав администратора
        [Test]
        public void IsAdmin_AdminUser_ReturnsTrue()
        {
            // Arrange
            string username = "admin";

            // Act
            bool result = _authService.IsAdmin(username);

            // Assert
            Assert.IsTrue(result, "Пользователь с ролью Administrator должен быть администратором.");
        }

        // Тест 3: Проверка пересечения обязанностей
        [Test]
        public void HasOverlappingDuties_OverlappingDates_ReturnsTrue()
        {
            // Arrange
            _context.Duties.Add(new Duty
            {
                Id = 1,
                UserId = 1,
                TaskId = 1,
                Name = "Duty1",
                StartDate = new DateTime(2025, 4, 1),
                EndDate = new DateTime(2025, 4, 5),
                Duration = 4
            });
            _context.SaveChanges();

            // Act
            bool result = _authService.HasOverlappingDuties(1, new DateTime(2025, 4, 3), new DateTime(2025, 4, 7));

            // Assert
            Assert.IsTrue(result, "Обязанности с пересекающимися датами должны быть обнаружены.");
        }

        [Test]
        public void HasOverlappingDuties_NoOverlap_ReturnsFalse()
        {
            // Arrange
            _context.Duties.Add(new Duty
            {
                Id = 1,
                UserId = 1,
                TaskId = 1,
                Name = "Duty1",
                StartDate = new DateTime(2025, 4, 1),
                EndDate = new DateTime(2025, 4, 5),
                Duration = 4
            });
            _context.SaveChanges();

            // Act
            bool result = _authService.HasOverlappingDuties(1, new DateTime(2025, 4, 6), new DateTime(2025, 4, 10));

            // Assert
            Assert.IsFalse(result, "Обязанности без пересечения дат не должны быть обнаружены.");
        }

        [Test]
        public void CreateDuty_WithOverlap_ReturnsFalse()
        {
            // Arrange
            _context.Duties.Add(new Duty
            {
                Id = 1,
                UserId = 1,
                TaskId = 1,
                Name = "Duty1",
                StartDate = new DateTime(2025, 4, 1),
                EndDate = new DateTime(2025, 4, 5),
                Duration = 4
            });
            _context.SaveChanges();

            string name = "NewDuty";
            DateTime startDate = new DateTime(2025, 4, 3);
            DateTime endDate = new DateTime(2025, 4, 7);
            int taskId = 1;
            int userId = 1;

            // Act
            bool result = _authService.CreateDuty(name, startDate, endDate, taskId, userId);

            // Assert
            Assert.IsFalse(result, "Создание обязанности с пересечением дат должно завершиться неудачей.");
        }

        // Тест 6: Поиск сотрудников
        [Test]
        public void SearchEmployees_ValidSearch_ReturnsMatchingEmployees()
        {
            // Arrange
            string searchText = "Admin User";
            int userId = 1;
            bool isAdmin = true;

            // Act
            var result = _authService.SearchEmployees(searchText, userId, isAdmin);

            // Assert
            Assert.AreEqual(1, result.Count, "Должен быть найден один сотрудник с именем 'Admin User'.");
            Assert.AreEqual("Admin User", result[0].FullName, "Найденный сотрудник должен соответствовать критерию поиска.");
        }



        // Переопределение EmployeeDbContext для использования In-Memory базы
        public class EmployeeDbContext : DbContext
        {
            public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

            public DbSet<Employee> Employees { get; set; }
            public DbSet<PositionEmp> Positions { get; set; }
            public DbSet<Task> Tasks { get; set; }
            public DbSet<Duty> Duties { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Employee>().ToTable("Employees");
                modelBuilder.Entity<PositionEmp>().ToTable("Positions");
                modelBuilder.Entity<Task>().ToTable("Tasks");
                modelBuilder.Entity<Duty>().ToTable("Duties");
            }
        }
    }
}