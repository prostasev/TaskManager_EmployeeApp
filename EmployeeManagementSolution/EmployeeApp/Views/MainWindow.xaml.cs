using EmployeeApp.ViewModels;
using System.Windows;

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
    }
}
