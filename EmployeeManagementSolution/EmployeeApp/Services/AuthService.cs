using EmployeeApp.Data;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;
using Task = EmployeeApp.Models.Task;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeApp.Services
{
    public class AuthService
    {
        public bool Authenticate(string username, string password)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    var employee = context.Employees
                        .Include(e => e.Position)
                        .FirstOrDefault(e => e.Username == username && e.PasswordHash == password);

                    return employee != null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при аутентификации: {ex.Message}", ex);
            }
        }

        public bool IsAdmin(string username)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    var employee = context.Employees
                        .Include(e => e.Position)
                        .FirstOrDefault(e => e.Username == username);
                    return employee?.Position?.Position == "Administrator";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при проверке прав администратора: {ex.Message}", ex);
            }
        }

        public bool HasOverlappingDuties(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    return context.Duties
                        .Any(d => d.UserId == userId &&
                                  d.StartDate < endDate &&
                                  d.EndDate > startDate);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при проверке пересечения обязанностей: {ex.Message}", ex);
            }
        }

        public List<Duty> GetEmployeeDuties(int userId)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    return context.Duties
                        .Include(d => d.Task)
                        .Include(d => d.Employee)
                        .Where(d => d.UserId == userId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении обязанностей сотрудника: {ex.Message}", ex);
            }
        }

        public List<Duty> GetDutiesByTask(int taskId)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    return context.Duties
                        .Include(d => d.Task)
                        .Include(d => d.Employee)
                        .Where(d => d.TaskId == taskId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении обязанностей по задаче: {ex.Message}", ex);
            }
        }

        public void CreateTask(string name, string? description)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    var task = new Task { Name = name, Description = description };
                    context.Tasks.Add(task);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании задачи: {ex.Message}", ex);
            }
        }

        public bool CreateDuty(string name, DateTime startDate, DateTime endDate, int taskId, int userId)
        {
            try
            {
                if (HasOverlappingDuties(userId, startDate, endDate))
                {
                    return false;
                }

                using (var context = new EmployeeDbContext())
                {
                    var duty = new Duty
                    {
                        Name = name,
                        StartDate = startDate,
                        EndDate = endDate,
                        Duration = (endDate - startDate).Days,
                        TaskId = taskId,
                        UserId = userId
                    };
                    context.Duties.Add(duty);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании обязанности: {ex.Message}", ex);
            }
        }

        public void UpdateTask(int taskId, string name, string? description)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    var task = context.Tasks.Find(taskId);
                    if (task != null)
                    {
                        task.Name = name;
                        task.Description = description;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении задачи: {ex.Message}", ex);
            }
        }

        public void DeleteTask(int taskId)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    var task = context.Tasks.Find(taskId);
                    if (task != null)
                    {
                        context.Tasks.Remove(task);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении задачи: {ex.Message}", ex);
            }
        }

        public void UpdateDuty(int dutyId, string name, DateTime startDate, DateTime endDate, int taskId, int userId)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    var duty = context.Duties.Find(dutyId);
                    if (duty != null)
                    {
                        if (HasOverlappingDuties(userId, startDate, endDate))
                        {
                            throw new InvalidOperationException("Нельзя обновить обязанность: даты пересекаются.");
                        }

                        duty.Name = name;
                        duty.StartDate = startDate;
                        duty.EndDate = endDate;
                        duty.Duration = (endDate - startDate).Days;
                        duty.TaskId = taskId;
                        duty.UserId = userId;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении обязанности: {ex.Message}", ex);
            }
        }

        public void DeleteDuty(int dutyId)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    var duty = context.Duties.Find(dutyId);
                    if (duty != null)
                    {
                        context.Duties.Remove(duty);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении обязанности: {ex.Message}", ex);
            }
        }

        public List<Employee> SearchEmployees(string? searchText)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    return context.Employees
                        .Include(e => e.Position)
                        .Where(e => string.IsNullOrEmpty(searchText) || e.FullName.Contains(searchText))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при поиске сотрудников: {ex.Message}", ex);
            }
        }

        public List<Task> SearchTasks(string? searchText)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    return context.Tasks
                        .Where(t => string.IsNullOrEmpty(searchText) || t.Name.Contains(searchText))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при поиске задач: {ex.Message}", ex);
            }
        }

        public List<Duty> SearchDuties(string? searchText)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    return context.Duties
                        .Include(d => d.Task)
                        .Include(d => d.Employee)
                        .Where(d => string.IsNullOrEmpty(searchText) || d.Name.Contains(searchText))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при поиске обязанностей: {ex.Message}", ex);
            }
        }

        public List<Duty> FilterDutiesByDate(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    var query = context.Duties
                        .Include(d => d.Task)
                        .Include(d => d.Employee)
                        .AsQueryable();

                    if (startDate.HasValue)
                    {
                        query = query.Where(d => d.StartDate >= startDate.Value);
                    }

                    if (endDate.HasValue)
                    {
                        query = query.Where(d => d.EndDate <= endDate.Value);
                    }

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при фильтрации обязанностей: {ex.Message}", ex);
            }
        }

        public List<Task> GetAllTasks()
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    return context.Tasks.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении всех задач: {ex.Message}", ex);
            }
        }

        public List<Employee> GetAllEmployees()
        {
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    return context.Employees.Include(e => e.Position).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении всех сотрудников: {ex.Message}", ex);
            }
        }
    }
}
