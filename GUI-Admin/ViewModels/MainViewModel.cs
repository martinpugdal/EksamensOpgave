using BLL;
using DTO.Models;
using GUI_Admin.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GUI_Admin.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Employee _selectedEmployee;
        private Department _selectedDepartment;

        private Employee _newEmployee = new();
        private string _newDepartmentName;
        public ObservableCollection<Employee> CurrentEmployeeList { get; set; } = new();
        public ObservableCollection<Department> DepartmentList { get; set; } = new();

        private ObservableCollection<Employee> _employeesNotInSelectedDepartment = new();

        public bool IsDepartmentSelected => SelectedDepartment != null;

        public bool IsDepartmentAndEmployeeSelected => SelectedDepartment != null && SelectedEmployee != null;

        public Employee NewEmployee
        {
            get => _newEmployee;
            set
            {
                if (value == null) return;

                if (_newEmployee.Name != value.Name || _newEmployee.Cpr != value.Cpr)
                {
                    _newEmployee = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string NewDepartmentName
        {
            get => _newDepartmentName;
            set
            {
                if (_newDepartmentName != value)
                {
                    _newDepartmentName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IsDepartmentAndEmployeeSelected));
            }
        }
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                if (_selectedDepartment != value)
                {
                    _selectedDepartment = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(IsDepartmentSelected));
                    NotifyPropertyChanged(nameof(IsDepartmentAndEmployeeSelected));
                    LoadEmployeesNotInSelectedDepartment();
                }
            }
        }


        public ICommand AddEmployeeToDepartmentCommand { get; }
        public ICommand CreateEmployeeCommand { get; }
        public ICommand CreateDepartmentCommand { get; }
        public ICommand ShowMoreForSelectedDepartmentCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            AddEmployeeToDepartmentCommand = new Command(AddEmployeeToDepartment);
            CreateEmployeeCommand = new Command(CreateEmployee);
            CreateDepartmentCommand = new Command(CreateDepartment);
            ShowMoreForSelectedDepartmentCommand = new Command(ShowDepartmentPageForSelectedDepartment);

            LoadDepartments();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            CurrentEmployeeList.Clear();
            foreach (var employee in EmployeeLogic.GetEmployees())
            {
                CurrentEmployeeList.Add(employee);
            }
            NotifyPropertyChanged(nameof(CurrentEmployeeList));
        }

        private void LoadDepartments()
        {
            DepartmentList.Clear();
            var departments = DepartmentLogic.GetDepartments();
            departments.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal)); //Sørger for alfabetisk rækkefølge i visuel repræsentation
            foreach (var department in departments)
            {
                DepartmentList.Add(department);
            }
        }

        private void AddEmployeeToDepartment()
        {
            if (SelectedEmployee != null && SelectedDepartment != null)
            {
                DepartmentLogic.AddEmployeeToDepartment(SelectedDepartment.Id, SelectedEmployee.Id);
                LoadEmployeesNotInSelectedDepartment();

                var tempDepartment = SelectedDepartment;
                LoadDepartments();
                LoadEmployees();

                SelectedDepartment = DepartmentList.FirstOrDefault(d => d.Id == tempDepartment.Id);

                Application.Current.MainPage.DisplayAlert("Info", "Medarbejder er tilføjet til afdeling.", "OK");
            }
        }

        private void LoadEmployeesNotInSelectedDepartment()
        {
            CurrentEmployeeList.Clear();
            if (SelectedDepartment != null)
            {
                List<int> EmployeesID = new();
                foreach (Employee employee in SelectedDepartment.Employees)
                {
                    EmployeesID.Add(employee.Id);
                }

                foreach (Employee employee in EmployeeLogic.GetEmployees())
                {
                    if (!EmployeesID.Contains(employee.Id))
                    {
                        CurrentEmployeeList.Add(employee);
                    }
                }
            }
        }

        private void CreateEmployee()
        {
            if (!string.IsNullOrEmpty(NewEmployee.Name) && !string.IsNullOrEmpty(NewEmployee.Cpr))
            {
                EmployeeLogic.CreateEmployee(NewEmployee.Name, NewEmployee.Cpr);
                LoadEmployees();
                NewEmployee = new();

                Application.Current.MainPage.DisplayAlert("Info", "Medarbejder er oprettet.", "OK");
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Info", "Udfyld venligst alle felter.", "OK");
            }
        }

        private void CreateDepartment()
        {
            if (!string.IsNullOrWhiteSpace(NewDepartmentName))
            {
                DepartmentLogic.CreateDepartment(NewDepartmentName);
                LoadDepartments();
                NewDepartmentName = string.Empty;

                Application.Current.MainPage.DisplayAlert("Info", "Afdelingen er oprettet.", "OK");
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Info", "Udfyld venligst alle felter.", "OK");
            }
        }

        private void ShowDepartmentPageForSelectedDepartment()
        {
            if (SelectedDepartment != null)
            {
                Application.Current.MainPage.Navigation.PushAsync(new DepartmentPage(SelectedDepartment));
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
