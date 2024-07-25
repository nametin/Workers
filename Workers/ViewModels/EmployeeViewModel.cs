
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Workers.Models;
using Workers.Views;
using System.Windows;
using System.Configuration;
using Workers.Core.Interfaces;

namespace Workers.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly IEmployeeService _employeeService;

        public ObservableCollection<Employee> PendingPositions { get; set; }
        public ObservableCollection<ActiveEmployee> ActiveEmployees { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand CleanCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
                ((RelayCommand)RemoveCommand).RaiseCanExecuteChanged();
            }
        }

        public string ActiveEmployeeFilePath { get; set; }

        /*
         file path'teki xml dosyası backend gibi çalışır, save/load fonksiyonlarında kullanılır
         pendingpositions üzerinde direkt değişiklik yapılacak listeyi
         active employees, üzerinde dolaylı (pending positions işlenerek) değişiklik yapılacak listeyi temsil eder
         anasayfanın açılışında aktif çalışanların görünmesi amaçlı loademployee methodu constructorda çağrılır
         */
        public EmployeeViewModel(IEmployeeService employeeService)
        {
            ActiveEmployeeFilePath = ConfigurationManager.AppSettings["ActiveEmployeeFilePath"];

            PendingPositions = new ObservableCollection<Employee>();
            ActiveEmployees = new ObservableCollection<ActiveEmployee>();

            AddCommand = new RelayCommand(AddEmployee, CanAddEmployee);
            EditCommand = new RelayCommand(() => EditEmployee(SelectedEmployee), CanEditOrRemoveEmployee);
            RemoveCommand = new RelayCommand(() => RemoveEmployee(SelectedEmployee), CanEditOrRemoveEmployee);
            CleanCommand = new RelayCommand(CleanEmployees);
            LoadCommand = new RelayCommand(LoadEmployees);
            SaveCommand = new RelayCommand(SaveEmployees);

            PendingPositions.CollectionChanged += (s, e) =>
            {
                ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
                ((RelayCommand)RemoveCommand).RaiseCanExecuteChanged();
            };
            _employeeService = employeeService;

            //Ekstra ekledim, constructor içerisinde LoadEmployees çağrılırsa yükle butonuna gerek kalmaz. Yine de yükle butonunu kaldırmadım. 
            LoadEmployees();
        }

        /*pending positions listesine ekleme yapmak için window açar, eklenecek elemanı service methoduna gönderir*/
        private void AddEmployee()
        {
            var addEmployeeWindow = new AddEmployeeWindow();
            if (addEmployeeWindow.ShowDialog() == true)
            {
                var newEmployee = new Employee
                {
                    Id = PendingPositions.Count + 1,
                    FirstName = addEmployeeWindow.FirstName,
                    LastName = addEmployeeWindow.LastName,
                    Position = addEmployeeWindow.SelectedPosition,
                    Experience = addEmployeeWindow.Experience,
                    Status = addEmployeeWindow.SelectedStatus
                };

                try
                {
                    _employeeService.AddEmployee(newEmployee, PendingPositions, ActiveEmployees);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        /*pending positions listesindeki seçili elemanı düzenlemek için window açar, düenlenecek elemanı service methoduna gönderir*/
        private void EditEmployee(Employee selectedEmployee)
        {
            var editEmployeeWindow = new EditEmployeeWindow(selectedEmployee);

            if (editEmployeeWindow.ShowDialog() == true)
            {
                var editedEmployee = editEmployeeWindow.EditedEmployee;
                try
                {
                    _employeeService.EditEmployee(editedEmployee, PendingPositions, ActiveEmployees);
                    Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Güncelleme Hatası", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /*seçili elemanı silmek üzere service methoduna gönderir*/
        private void RemoveEmployee(Employee selectedEmployee)
        {
            try
            {
                _employeeService.RemoveEmployee(selectedEmployee, PendingPositions);
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Silinemedi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*pending listeyi temizlemek üzere service methoduna gönderirr*/
        private void CleanEmployees()
        {
            _employeeService.CleanEmployees(PendingPositions);
            OnPropertyChanged(nameof(PendingPositions));
        }

        /*ActiveEmployeeFilePath config dosyasından çekilmiş xml dosya yolu olmak üzere
          bu pathta bulnan xml dosyasından verileri active employees listesine doldurmak amaçlı iki veriyi service methoduna yollar.
         */
        private void LoadEmployees()
        {
            _employeeService.LoadEmployees(ActiveEmployeeFilePath, ActiveEmployees);
            //MessageBox.Show("Çalışanlar başarıyla import edildi", "Import Edildi", MessageBoxButton.OK, MessageBoxImage.Information);
            Refresh();
        }

        /*
         ActiveEmployeeFilePath config dosyasından çekilmiş xml dosya yolu olmak üzere
         pending positions listesini active employees listesine işleyerek active employees listesinin son halini xml dosyasına kaydeder
         */
        private void SaveEmployees()
        {
            try
            {
                _employeeService.SaveEmployees(PendingPositions, ActiveEmployees, ActiveEmployeeFilePath);
                MessageBox.Show("Çalışanlar başarıyla kaydedildi", "Kaydedildi", MessageBoxButton.OK, MessageBoxImage.Information);
                PendingPositions.Clear();
                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Kaydetme Hatası", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*arayüzü yeniler*/
        private void Refresh()
        {
            OnPropertyChanged(nameof(PendingPositions));
            OnPropertyChanged(nameof(ActiveEmployees));
            OnPropertyChanged(nameof(SelectedEmployee));
        }

        /*eğer pending positions'a eklenmiş kişi sayısı 20 olduysa artık yeni ekleme yapılamaz*/
        private bool CanAddEmployee()
        {
            return PendingPositions.Count < 20;
        }

        /*herhangi bir kişi seçilemediyse edit veya remove tuşları pasif olur*/
        private bool CanEditOrRemoveEmployee()
        {
            return SelectedEmployee != null ;
        }

        /*private bool CanRemoveEmployee(  )
        {
            return SelectedEmployee != null && PendingPositions.Count != 0;
        }*/

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
