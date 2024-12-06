using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO.Models
{
    public class Employee : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string Cpr { get; set; }


        public string Name { get; set; }
        public List<Department> Departments { get; set; } = new List<Department>();

        public List<Timetracker> Timetrackers { get; set; } = new List<Timetracker>();

        private double _totalHours;
        public double TotalHours
        {
            get => _totalHours;
            set
            {
                if (_totalHours != value)
                {
                    _totalHours = value;
                    OnPropertyChanged();
                }
            }
        }

        public Employee()
        {
        }

        public Employee(int employeeId, string employeeCpr, string employeeName)
        {
            Id = employeeId;
            Cpr = employeeCpr;
            Name = employeeName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
