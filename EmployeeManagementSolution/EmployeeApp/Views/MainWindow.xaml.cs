using EmployeeApp.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace EmployeeApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string username)
        {
            InitializeComponent();
            DataContext = new MainViewModel(username); 
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool isFullScreen = false;

        private void ToggleFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (!isFullScreen)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
            }
            else
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResizeWithGrip;
            }
            isFullScreen = !isFullScreen;
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

    }
}
