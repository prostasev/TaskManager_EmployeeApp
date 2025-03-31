using EmployeeApp.Models;
using EmployeeApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Task = EmployeeApp.Models.Task;
using System.Diagnostics; 

namespace EmployeeApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly int _currentUserId;

        public bool IsAdmin { get; }
        public ObservableCollection<Duty> Duties { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        public string? NewTaskName { get; set; } = string.Empty;
        public string? NewTaskDescription { get; set; } = string.Empty;
        public string? NewDutyName { get; set; } = string.Empty;
        public DateTime NewDutyStartDate { get; set; } = DateTime.Now;
        public DateTime NewDutyEndDate { get; set; } = DateTime.Now.AddDays(1);
        public Task? SelectedTask { get; set; }
        public Employee? SelectedEmployee { get; set; }
        public string? SearchEmployeeText { get; set; } = string.Empty;
        public string? SearchTaskText { get; set; } = string.Empty;
        public string? SearchDutyText { get; set; } = string.Empty;
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }

        public ICommand CreateTaskCommand { get; }
        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand CreateDutyCommand { get; }
        public ICommand UpdateDutyCommand { get; }
        public ICommand DeleteDutyCommand { get; }
        public ICommand SearchEmployeesCommand { get; }
        public ICommand SearchTasksCommand { get; }
        public ICommand SearchDutiesCommand { get; }
        public ICommand FilterDutiesCommand { get; }

        public MainViewModel(string username)
        {
            try
            {
                _authService = new AuthService();
                IsAdmin = _authService.IsAdmin(username);

                using (var context = new Data.EmployeeDbContext())
                {
                    var employee = context.Employees.FirstOrDefault(e => e.Username == username);
                    _currentUserId = employee?.Id ?? 0;
                }

                Duties = new ObservableCollection<Duty>(_authService.GetEmployeeDuties(_currentUserId));
                Tasks = new ObservableCollection<Task>(_authService.GetAllTasks(_currentUserId, IsAdmin));
                Employees = new ObservableCollection<Employee>(_authService.GetAllEmployees(_currentUserId, IsAdmin)); 

                CreateTaskCommand = new RelayCommand(CreateTask, () => IsAdmin);
                UpdateTaskCommand = new RelayCommand(UpdateTask, () => IsAdmin);
                DeleteTaskCommand = new RelayCommand(DeleteTask, () => IsAdmin);
                CreateDutyCommand = new RelayCommand(CreateDuty, () => IsAdmin);
                UpdateDutyCommand = new RelayCommand(UpdateDuty, () => IsAdmin);
                DeleteDutyCommand = new RelayCommand(DeleteDuty, () => IsAdmin);
                SearchEmployeesCommand = new RelayCommand(SearchEmployees);
                SearchTasksCommand = new RelayCommand(SearchTasks);
                SearchDutiesCommand = new RelayCommand(SearchDuties);
                FilterDutiesCommand = new RelayCommand(FilterDuties);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Инициализация MainViewModel: {ex}");
                MessageBox.Show($"Ошибка при инициализации: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private void CreateTask()
        {
            try
            {
                if (!string.IsNullOrEmpty(NewTaskName))
                {
                    _authService.CreateTask(NewTaskName, NewTaskDescription);
                    Tasks = new ObservableCollection<Task>(_authService.GetAllTasks(_currentUserId, IsAdmin)); 
                    OnPropertyChanged(nameof(Tasks));
                    NewTaskName = string.Empty;
                    NewTaskDescription = string.Empty;
                    OnPropertyChanged(nameof(NewTaskName));
                    OnPropertyChanged(nameof(NewTaskDescription));
                }
                else
                {
                    MessageBox.Show("Название задачи не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CreateTask: {ex}");
                MessageBox.Show($"Ошибка при создании задачи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTask()
        {
            try
            {
                if (SelectedTask != null)
                {
                    _authService.UpdateTask(SelectedTask.Id, SelectedTask.Name ?? string.Empty, SelectedTask.Description);
                    Tasks = new ObservableCollection<Task>(_authService.GetAllTasks(_currentUserId, IsAdmin)); 
                    OnPropertyChanged(nameof(Tasks));
                }
                else
                {
                    MessageBox.Show("Выберите задачу для обновления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateTask: {ex}");
                MessageBox.Show($"Ошибка при обновлении задачи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTask()
        {
            try
            {
                if (SelectedTask != null)
                {
                    _authService.DeleteTask(SelectedTask.Id);
                    Tasks = new ObservableCollection<Task>(_authService.GetAllTasks(_currentUserId, IsAdmin)); 
                    OnPropertyChanged(nameof(Tasks));
                }
                else
                {
                    MessageBox.Show("Выберите задачу для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteTask: {ex}");
                MessageBox.Show($"Ошибка при удалении задачи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateDuty()
        {
            try
            {
                if (!string.IsNullOrEmpty(NewDutyName) && SelectedTask != null && SelectedEmployee != null)
                {
                    var success = _authService.CreateDuty(NewDutyName, NewDutyStartDate, NewDutyEndDate, SelectedTask.Id, SelectedEmployee.Id);
                    if (success)
                    {
                        Duties = new ObservableCollection<Duty>(_authService.GetEmployeeDuties(_currentUserId));
                        OnPropertyChanged(nameof(Duties));
                        NewDutyName = string.Empty;
                        OnPropertyChanged(nameof(NewDutyName));
                    }
                    else
                    {
                        MessageBox.Show("Не удалось создать обязанность. Возможно, даты пересекаются.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Заполните все обязательные поля: выберите задачу и сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CreateDuty: {ex}");
                MessageBox.Show($"Ошибка при создании обязанности: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateDuty()
        {
            try
            {
                if (SelectedDuty != null)
                {
                    _authService.UpdateDuty(SelectedDuty.UserId, SelectedDuty.Name ?? string.Empty, SelectedDuty.StartDate, SelectedDuty.EndDate, SelectedDuty.TaskId, SelectedDuty.UserId);
                    Duties = new ObservableCollection<Duty>(_authService.GetEmployeeDuties(_currentUserId));
                    OnPropertyChanged(nameof(Duties));
                }
                else
                {
                    MessageBox.Show("Выберите обязанность для обновления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateDuty: {ex}");
                MessageBox.Show($"Ошибка при обновлении обязанности: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteDuty()
        {
            try
            {
                if (SelectedDuty != null)
                {
                    _authService.DeleteDuty(SelectedDuty.UserId);
                    Duties = new ObservableCollection<Duty>(_authService.GetEmployeeDuties(_currentUserId));
                    OnPropertyChanged(nameof(Duties));
                }
                else
                {
                    MessageBox.Show("Выберите обязанность для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteDuty: {ex}");
                MessageBox.Show($"Ошибка при удалении обязанности: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchEmployees()
        {
            try
            {
                Employees = new ObservableCollection<Employee>(_authService.SearchEmployees(SearchEmployeeText, _currentUserId, IsAdmin));
                OnPropertyChanged(nameof(Employees));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SearchEmployees: {ex}");
                MessageBox.Show($"Ошибка при поиске сотрудников: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTasks()
        {
            try
            {
                Tasks = new ObservableCollection<Task>(_authService.SearchTasks(SearchTaskText, _currentUserId, IsAdmin));
                OnPropertyChanged(nameof(Tasks));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SearchTasks: {ex}");
                MessageBox.Show($"Ошибка при поиске задач: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        

        private void SearchDuties()
        {
            try
            {
                Duties = new ObservableCollection<Duty>(_authService.SearchDuties(SearchDutyText, _currentUserId, IsAdmin));
                OnPropertyChanged(nameof(Duties));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SearchDuties: {ex}");
                MessageBox.Show($"Ошибка при поиске обязанностей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterDuties()
        {
            try
            {
                Duties = new ObservableCollection<Duty>(_authService.FilterDutiesByDate(FilterStartDate, FilterEndDate, _currentUserId, IsAdmin));
                OnPropertyChanged(nameof(Duties));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FilterDuties: {ex}");
                MessageBox.Show($"Ошибка при фильтрации обязанностей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Duty? _selectedDuty;
        public Duty? SelectedDuty
        {
            get => _selectedDuty;
            set
            {
                _selectedDuty = value;
                OnPropertyChanged(nameof(SelectedDuty));
            }
        }
    }

    public class BaseViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;
        private event EventHandler? _canExecuteChanged;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { _canExecuteChanged += value; }
            remove { _canExecuteChanged -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
        public void Execute(object? parameter) => _execute();

        public void RaiseCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}