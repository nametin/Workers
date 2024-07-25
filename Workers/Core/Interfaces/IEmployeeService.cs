
using System.Collections.ObjectModel;
using Workers.Models;

namespace Workers.Core.Interfaces
{
    public interface IEmployeeService
    {
        void AddEmployee(Employee employee, ObservableCollection<Employee> pendingPositions, ObservableCollection<ActiveEmployee> activeEmployees);
        void EditEmployee(Employee employee, ObservableCollection<Employee> pendingPositions, ObservableCollection<ActiveEmployee> activeEmployees);
        void RemoveEmployee(Employee employee, ObservableCollection<Employee> pendingPositions);
        void CleanEmployees(ObservableCollection<Employee> pendingPositions);
        void LoadEmployees(string filePath, ObservableCollection<ActiveEmployee> activeEmployees);
        void SaveEmployees(ObservableCollection<Employee> pendingPositions, ObservableCollection<ActiveEmployee> activeEmployees, string filePath);
    }
}
