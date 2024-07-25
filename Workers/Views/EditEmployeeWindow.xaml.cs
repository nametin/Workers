using System.Windows;
using Workers.Models;
using Workers.Util;

namespace Workers.Views
{
    public partial class EditEmployeeWindow : Window
    {
        //private Employee originalEmployee;
        public Employee EditedEmployee { get; private set; }

        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
           //originalEmployee = employee;

            FirstNameTextBox.Text = employee.FirstName;
            LastNameTextBox.Text = employee.LastName;
            PositionComboBox.SelectedIndex = (int)employee.Position;
            ExperienceTextBox.Text = employee.Experience.ToString();
            StatusComboBox.SelectedIndex = (int)employee.Status;

            EditedEmployee = new Employee
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position = employee.Position,
                Experience = employee.Experience,
                Status = employee.Status
            };
        }

        /*düzenlenecek (düzenlenmiş hali) eleman valid ise DialogResult true olur.
        msg hata mesajıdır. eğer hata mesajı uzunluğu 0 ise (hata yok ise) DialogResult true olmalıdır.
        */
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string msg = ValidationHelper.IsValidInput((Position)PositionComboBox.SelectedIndex, int.TryParse(ExperienceTextBox.Text, out int ex) ? ex : -1, FirstNameTextBox.Text, LastNameTextBox.Text);

            if (msg.Length == 0)
            {
                EditedEmployee.FirstName = FirstNameTextBox.Text;
                EditedEmployee.LastName = LastNameTextBox.Text;
                EditedEmployee.Position = (Position)PositionComboBox.SelectedIndex;
                EditedEmployee.Experience = int.TryParse(ExperienceTextBox.Text, out int exp) ? exp : 0;
                EditedEmployee.Status = (Status)StatusComboBox.SelectedIndex;

                DialogResult = true;
            }
            else
            {
                MessageBox.Show(msg, "Kullanıcı Güncellenemedi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public string FirstName => FirstNameTextBox.Text;
        public string LastName => LastNameTextBox.Text;
        public Position SelectedPosition => (Position)PositionComboBox.SelectedIndex;
        public int Experience => int.TryParse(ExperienceTextBox.Text, out int exp) ? exp : 0;
        public Status SelectedStatus => (Status)StatusComboBox.SelectedIndex;
    }
}
