using System.Windows;
using EmployeeApp.Services;

namespace EmployeeApp.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UsernamePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                UsernamePlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorTextBlock.Text = "Пожалуйста, заполните все поля.";
                return;
            }

            bool isAuthenticated = _authService.Authenticate(username, password);

            if (isAuthenticated)
            {
                var mainWindow = new MainWindow(username); // Передаем username
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErrorTextBlock.Text = "Неверный логин или пароль.";
            }
        }
    }

    }


