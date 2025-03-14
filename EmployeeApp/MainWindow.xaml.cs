using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MySql.Data.MySqlClient;

namespace EmployeeApp
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=localhost;Database=EmployeeDb;Uid=root;Pwd=281105;charset=utf8;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> allData = new List<string>();

                allData.Add("=== Позиции ===");
                allData.AddRange(GetPositions().Select(pos =>
                    $"ID: {pos.Id}, Название: {pos.Name}"));

                allData.Add("\n=== Пользователи ===");
                allData.AddRange(GetUsers().Select(user =>
                    $"ID: {user.Id}, {user.FullName}, Позиция: {user.PositionId}, Ставка: {user.Rate}, Логин: {user.Username}"));

                allData.Add("\n=== Задачи ===");
                allData.AddRange(GetTasks().Select(task =>
                    $"ID: {task.Id}, {task.Name}, {task.Description}"));

                allData.Add("\n=== Обязанности ===");
                allData.AddRange(GetDuties().Select(duty =>
                    $"ID: {duty.Id}, Задача: {duty.TaskId}, Пользователь: {duty.UserId}, {duty.Name}, {duty.StartDate} - {duty.EndDate}"));

                EmployeeListBox.ItemsSource = allData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private List<Position> GetPositions()
        {
            List<Position> positions = new List<Position>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Name FROM Positions";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        positions.Add(new Position
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name")
                        });
                    }
                }
            }
            return positions;
        }

        private List<User> GetUsers()
        {
            List<User> users = new List<User>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, FullName, PositionId, Rate, Username, PasswordHash FROM Users";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32("Id"),
                            FullName = reader.GetString("FullName"),
                            PositionId = reader.GetInt32("PositionId"),
                            Rate = reader.GetDecimal("Rate"),
                            Username = reader.GetString("Username"),
                            PasswordHash = reader.GetString("PasswordHash")
                        });
                    }
                }
            }
            return users;
        }

        private List<TaskData> GetTasks()
        {
            List<TaskData> tasks = new List<TaskData>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Name, Description FROM Tasks";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskData
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Description = reader.IsDBNull(2) ? "Нет описания" : reader.GetString("Description")
                        });
                    }
                }
            }
            return tasks;
        }

        private List<Duty> GetDuties()
        {
            List<Duty> duties = new List<Duty>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, TaskId, UserId, Name, StartDate, EndDate FROM Duties";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        duties.Add(new Duty
                        {
                            Id = reader.GetInt32("Id"),
                            TaskId = reader.GetInt32("TaskId"),
                            UserId = reader.GetInt32("UserId"),
                            Name = reader.GetString("Name"),
                            StartDate = reader.GetDateTime("StartDate"),
                            EndDate = reader.GetDateTime("EndDate")
                        });
                    }
                }
            }
            return duties;
        }
    }

    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int PositionId { get; set; }
        public decimal Rate { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public class TaskData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Duty
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}