using BLL;
using DTO.Models;
using GUI_Admin.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GUI_Admin.ViewModels
{
    public class DepartmentViewModel : INotifyPropertyChanged
    {
        private Department _department;
        public Department Department
        {
            get => _department;
            set
            {
                if (_department != value)
                {
                    _department = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                if (_selectedEmployee != value)
                {
                    _selectedEmployee = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(IsEmployeeSelected));
                }
            }
        }

        public bool IsEmployeeSelected => SelectedEmployee != null;


        private Case _newCase = new();

        public ObservableCollection<Case> CaseList { get; set; } = new();
        public ObservableCollection<Employee> EmployeeList { get; set; } = new();

        public Case NewCase
        {
            get => _newCase;
            set
            {
                if (value == null) return;

                if (_newCase.Title != value.Title || _newCase.Description != value.Description)
                {
                    _newCase = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _currentFilter;
        public string CurrentFilter
        {
            get => _currentFilter;
            set
            {
                if (_currentFilter != value)
                {
                    _currentFilter = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void UpdateTimeData(TimeFilter filter)
        {
            List<Timetracker> timetrackers = new();
            switch (filter)
            {
                case TimeFilter.Weekly:
                    timetrackers = TimetrackerLogic.GetWeeklyTimetrackersForDepartment(Department.Id);
                    CurrentFilter = "Uge";
                    break;
                case TimeFilter.Monthly:
                    timetrackers = TimetrackerLogic.GetMonthlyTimetrackersForDepartment(Department.Id);
                    CurrentFilter = "Måned";
                    break;
                case TimeFilter.Total:
                    timetrackers = TimetrackerLogic.GetTotalTimetrackersForDepartment(Department.Id);
                    CurrentFilter = "Total";
                    break;
            }

            foreach (var e in EmployeeList)
                e.TotalHours = 0;

            foreach (var tracker in timetrackers)
            {
                var employee = EmployeeList.FirstOrDefault(e => e.Id == tracker.EmployeeId);
                if (employee == null || tracker.DateTimeEnd == null) continue;

                var timespan = tracker.DateTimeEnd - tracker.DateTimeStart;
                employee.TotalHours += timespan.Value.TotalHours;
            }

            foreach (var e in EmployeeList)
                e.TotalHours = Math.Round(e.TotalHours, 2);
        }

        public enum TimeFilter
        {
            Weekly,
            Monthly,
            Total
        }

        public ICommand CreateCaseCommand { get; }
        public ICommand ShowWeeklyTimeCommand { get; }
        public ICommand ShowMonthlyTimeCommand { get; }
        public ICommand ShowTotalTimeCommand { get; }

        public ICommand ShowMoreAboutEmployeeCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DepartmentViewModel()
        {
        }

        public DepartmentViewModel(Department selectedDepartment)
        {
            Department = selectedDepartment;

            CreateCaseCommand = new Command(CreateCase);
            ShowWeeklyTimeCommand = new Command(ShowWeeklyTime);
            ShowMonthlyTimeCommand = new Command(ShowMonthlyTime);
            ShowTotalTimeCommand = new Command(ShowTotalTime);
            ShowMoreAboutEmployeeCommand = new Command(ShowEmployeePageForSelectedEmployee);

            LoadCases();
            LoadEmployeesInDepartment();
            ShowTotalTime();
        }

        private void ShowWeeklyTime()
        {
            UpdateTimeData(TimeFilter.Weekly);
            NotifyPropertyChanged(nameof(EmployeeList));
        }

        private void ShowMonthlyTime()
        {
            UpdateTimeData(TimeFilter.Monthly);
            NotifyPropertyChanged(nameof(EmployeeList));
        }

        private void ShowTotalTime()
        {
            UpdateTimeData(TimeFilter.Total);
            NotifyPropertyChanged(nameof(EmployeeList));
        }

        private void LoadCases()
        {
            CaseList.Clear();

            foreach (var c in Department.Cases)
            {
                CaseList.Add(c);
            }
        }

        private void CreateCase()
        {
            if (NewCase.Title == null || NewCase.Description == null)
            {
                Application.Current.MainPage.DisplayAlert("Fejl", "Udfyld venligst alle felter", "OK");
                return;
            }

            CaseLogic.CreateCase(NewCase.Title, NewCase.Description, Department.Id);
            String newCaseTitle = NewCase.Title;
            NewCase = new Case();
            Department = DepartmentLogic.GetDepartment(Department.Id);
            LoadCases();

            Application.Current.MainPage.DisplayAlert("Info", newCaseTitle + " er blevet oprettet", "OK");
        }

        private void LoadEmployeesInDepartment()
        {
            EmployeeList.Clear();

            foreach (var e in Department.Employees)
            {
                EmployeeList.Add(e);
            }
        }

        private void ShowEmployeePageForSelectedEmployee()
        {
            if (SelectedEmployee != null)
            {
                Application.Current.MainPage.Navigation.PushAsync(new EmployeePage(SelectedEmployee, Department));
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
