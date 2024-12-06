using BLL;
using DTO.Models;

namespace GUI_Employee.Models
{
    public class EmployeeAndDepartmentModel
    {
        public Employee Employee { get; set; }
        public Department Department { get; set; }
        public List<Timetracker> EmployeeTimetrackers { get; set; } = new List<Timetracker>();

        public EmployeeAndDepartmentModel(Employee employee, Department department)
        {
            Employee = employee;
            Department = department;
            var departmentTimetrackers = TimetrackerLogic.GetWeeklyTimetrackersForDepartment(department.Id).
                Where(t => t.EmployeeId == employee.Id);
            foreach (var timetracker in departmentTimetrackers)
            {
                EmployeeTimetrackers.Add(timetracker);
            }

            var timetrackers = TimetrackerLogic.GetWeeklyTimetrackersForEmployee(employee.Id).
                Where(t => t.DateTimeEnd != null).
                ToList();
            employee.TotalHours = timetrackers.
                Sum(t => (t.DateTimeEnd - t.DateTimeStart).Value.TotalHours);

            EmployeeTimetrackers.Sort((t1, t2) =>
            {
                if (t1.DateTimeEnd == null && t2.DateTimeEnd != null)
                    return -1;
                if (t1.DateTimeEnd != null && t2.DateTimeEnd == null)
                    return 1;
                return t1.DateTimeStart.CompareTo(t2.DateTimeStart);
            });
        }

        public Case? GetCase(int caseId)
        {
            return Department.Cases.Find(c => c.Id == caseId);
        }
    }
}
