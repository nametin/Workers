
using System.Collections.ObjectModel;
using Workers.Core.Interfaces;
using Workers.Models;
using Workers.Utils;


namespace Workers.Services
{
    public class EmployeeService : IEmployeeService
    {

        /*
            pending listede newEmployee zaten var mı diye kontrol edilir, eğer pending listede eleman varsa ekleme yapılmaz
            eğer çalışan işten çıkarılacaksa pending listede bulunmamalıdır
            eğer çalışan işe alınacaksa hem pending hem aktif listede bulunmamalıdır.
         */
        public void AddEmployee(Employee newEmployee, ObservableCollection<Employee> pendingPositions, ObservableCollection<ActiveEmployee> activeEmployees)
        {
            var existingPendingEmployee = pendingPositions
                    .FirstOrDefault(e => e.FirstName == newEmployee.FirstName &&
                                         e.LastName == newEmployee.LastName &&
                                         e.Position == newEmployee.Position &&
                                         e.Experience == newEmployee.Experience);

            if (existingPendingEmployee != null)
            {
                throw new System.Exception("Bu çalışan zaten mevcut");
            }

            if (newEmployee.Status == Status.Termination)
            {
                pendingPositions.Add(newEmployee);
            }
            else
            {
                var existingActiveEmployee = activeEmployees
                    .FirstOrDefault(e => e.FirstName == newEmployee.FirstName &&
                                         e.LastName == newEmployee.LastName &&
                                         e.Position == newEmployee.Position &&
                                         e.Experience == newEmployee.Experience);

                if (existingActiveEmployee != null)
                {
                    throw new System.Exception("Bu çalışan zaten aktif çalışanlar içerisinde mevcut");
                }
                
                pendingPositions.Add(newEmployee);
            }
        }

        /*
         Çalışanın pending veya active listelerde olup olmadığı kontrol ederek düzenlenir.
        Hiring durumundaki çalışan hem active hem pending listede olmamalı
        termination durumundaki çalışan pending listede olmamalı
        */
        public void EditEmployee(Employee editedEmployee, ObservableCollection<Employee> pendingPositions, ObservableCollection<ActiveEmployee> activeEmployees)
        {
            var employeeToUpdate = pendingPositions.FirstOrDefault(e => e.Id == editedEmployee.Id);
            if (employeeToUpdate != null)
            {
                var existingPendingEmployee = pendingPositions
                        .FirstOrDefault(e => e.FirstName == editedEmployee.FirstName &&
                                             e.LastName == editedEmployee.LastName &&
                                             e.Position == editedEmployee.Position &&
                                             e.Experience == editedEmployee.Experience &&
                                             e.Id != editedEmployee.Id);

                if (editedEmployee.Status == Status.Hiring)
                {
                    var existingActiveEmployee = activeEmployees
                        .FirstOrDefault(e => e.FirstName == editedEmployee.FirstName &&
                                             e.LastName == editedEmployee.LastName &&
                                             e.Position == editedEmployee.Position &&
                                             e.Experience == editedEmployee.Experience);

                    if (existingPendingEmployee != null || existingActiveEmployee != null)
                    {
                        throw new Exception("Bu bilgilerle güncelleme yapılamaz çünkü bu kişi zaten mevcut.");
                    }
                }
                else if (editedEmployee.Status == Status.Termination)                
                {
                    if (existingPendingEmployee != null)
                    {
                        throw new Exception("Bu bilgilerle güncelleme yapılamaz çünkü bu kişi zaten mevcut.");

                    }
                }
                employeeToUpdate.FirstName = editedEmployee.FirstName;
                employeeToUpdate.LastName = editedEmployee.LastName;
                employeeToUpdate.Position = editedEmployee.Position;
                employeeToUpdate.Experience = editedEmployee.Experience;
                employeeToUpdate.Status = editedEmployee.Status;
            }
        }

        /*yalnızca pending positions'ı temizler. active employees'e etki etmez*/
        public void CleanEmployees(ObservableCollection<Employee> pendingPositions)
        {
            pendingPositions.Clear();
        }

        /*Seçili çalışanı pending positions'tan siler. active employees'e etki etmze*/
        public void RemoveEmployee(Employee selectedEmployee, ObservableCollection<Employee> pendingPositions)
        {
            var employeeToRemove = pendingPositions.FirstOrDefault(e => e.Id == selectedEmployee.Id);
            if (employeeToRemove != null)
            {
                pendingPositions.Remove(employeeToRemove);
            }
            else
            {
                throw new Exception("Çalışan bulunamadı.");
            }
        }

        /*filePath: xml dosyasının konumu
         xml dosyasındaki her bir işçiyi activeEmployees listesine ekler.*/
        public void LoadEmployees(string filePath, ObservableCollection<ActiveEmployee> activeEmployees)
        {
            List<ActiveEmployee> loadedEmployees = XmlHelper.LoadFromXml(filePath);
            activeEmployees.Clear();
            foreach (var employee in loadedEmployees)
            {
                activeEmployees.Add(new ActiveEmployee
                {
                    Id = activeEmployees.Count + 1,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Position = employee.Position,
                    Experience = employee.Experience
                });
            }
        }

        /*filePath: xml dosyasının konumu,
         pending positions listesi: active employees dosyasına işlenecek işçileri içerir
         pending positions listesinde durumu hiring olan kişileri xml dosyasına ekler
         pending positions listesinde durumu termination olan kişileri xml dosyasından kaldırır
         */
        public void SaveEmployees(ObservableCollection<Employee> pendingPositions, ObservableCollection<ActiveEmployee> activeEmployees, string filePath)
        {
            List<Employee> pendingEmployees = pendingPositions.ToList();
            List<ActiveEmployee> currentActiveEmployees = activeEmployees.ToList();

            List<Employee> employeesToAdd = pendingEmployees.Where(e => e.Status == Status.Hiring).ToList();

            foreach (var employeeToAdd in employeesToAdd)
            {
                currentActiveEmployees.Add(new ActiveEmployee
                {
                    Id = employeeToAdd.Id,
                    FirstName = employeeToAdd.FirstName,
                    LastName = employeeToAdd.LastName,
                    Position = employeeToAdd.Position,
                    Experience = employeeToAdd.Experience
                });
            }

            List<Employee> employeesToRemove = pendingEmployees.Where(e => e.Status == Status.Termination).ToList();

            employeesToRemove.ForEach(employeeToRemove =>
            {
                var existingEmployee = currentActiveEmployees.FirstOrDefault(e => e.FirstName == employeeToRemove.FirstName &&
                                                                                 e.LastName == employeeToRemove.LastName &&
                                                                                 e.Position == employeeToRemove.Position &&
                                                                                 e.Experience == employeeToRemove.Experience);

                if (existingEmployee != null)
                {
                    currentActiveEmployees.Remove(existingEmployee);
                }
            });
            XmlHelper.SaveToXml(currentActiveEmployees, filePath);
        }
    }
}
