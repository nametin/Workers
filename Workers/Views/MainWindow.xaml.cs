
using System.Windows;
using Workers.ViewModels;
using Workers.Services;

namespace Workers.Views
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new EmployeeViewModel(new EmployeeService());
        }
    }
}
