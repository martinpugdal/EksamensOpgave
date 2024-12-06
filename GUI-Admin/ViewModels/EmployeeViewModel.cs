using DTO.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GUI_Admin.ViewModels
{
    internal class EmployeeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Employee Employee { get; }

        public Department Department { get; }
        public ObservableCollection<Timetracker> Timetrackers { get; set; } = new();

        public EmployeeViewModel()
        {
        }

        public EmployeeViewModel(Employee employee, Department department)
        {
            Employee = employee;
            Department = department;
            foreach (var timetracker in department.Timetrackers)
            {
                if (timetracker.EmployeeId != employee.Id) continue;
                Timetrackers.Add(timetracker);
            }
        }

        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}