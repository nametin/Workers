
using System.Windows;
using Workers.Models;
using Workers.Util;

namespace Workers.Views
{
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        /*eklenecek eleman valid ise DialogResult true olur.
         msg hata mesajıdır. eğer hata mesajı uzunluğu 0 ise (hata yok ise) DialogResult true olmalıdır.
         */
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
           
            string msg = ValidationHelper
                        .IsValidInput(
                        (Position)PositionComboBox.SelectedIndex, 
                        int.TryParse(ExperienceTextBox.Text, out int exp) ? exp : -1, 
                        FirstNameTextBox.Text, 
                        LastNameTextBox.Text
                        );
            if (msg.Length==0)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show(msg, "Kullanıcı Eklenemedi", MessageBoxButton.OK, MessageBoxImage.Error);
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
