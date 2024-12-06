using DAL.Repositories;
using DTO.Models;

namespace BLL
{
    public class CaseLogic
    {
        public static void CreateCase(string title, string description, int departmentId)
        {
            var @case = new Case()
            {
                Title = title,
                Description = description,
                DepartmentId = departmentId
            };

            CaseRepository.AddCase(@case);
        }
        //{
        //    Employee employee = EmployeeLogic.GetEmployee(employeeId);
        //    Department department = DepartmentLogic.GetDepartment(departmentId);
        //    Case? @case = department.Cases.FirstOrDefault(predicate: c => c.Id == caseId, defaultValue: null);

        //    var timetracker = new Timetracker()
        //    {
        //        EmployeeId = employeeId,
        //        CaseId = @case?.Id,
        //        DepartmentId = departmentId,
        //        DateTimeStart = startTime,
        //        DateTimeEnd = null
        //    };

        //    TimetrackerRepository.AddTimetracker(timetracker);
        //}
    }
}
