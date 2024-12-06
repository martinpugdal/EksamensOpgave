using DAL.Repositories;
using DTO.Models;

namespace BLL
{
    public class TimetrackerLogic
    {
        public static void CreateTimetracker(int employeeId, int departmentId, DateTime startTime, DateTime? endTime)
        {
            CreateTimetracker(employeeId, departmentId, null, startTime, endTime);
        }
        public static void CreateTimetracker(int employeeId, int departmentId, int? caseId, DateTime startTime, DateTime? endTime)
        {
            Employee employee = EmployeeLogic.GetEmployee(employeeId) ?? throw new ArgumentException("Employee not found");
            Department department = DepartmentLogic.GetDepartment(departmentId) ?? throw new ArgumentException("Department not found");
            Case? @case = department.Cases.FirstOrDefault(predicate: c => c.Id == caseId, defaultValue: null);

            var timetracker = new Timetracker()
            {
                EmployeeId = employeeId,
                CaseId = @case?.Id,
                DepartmentId = departmentId,
                DateTimeStart = startTime,
                DateTimeEnd = endTime
            };

            TimetrackerRepository.AddTimetracker(timetracker);
        }

        public static List<Timetracker> GetWeeklyTimetrackersForDepartment(int id)
        {
            var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
            {
                monday = monday.AddDays(-7);
            }
            var sunday = monday.AddDays(6);

            return TimetrackerRepository.GetTimetrackersForDepartment(id).
                Where(t => t.DateTimeStart >= monday && t.DateTimeStart <= sunday).ToList();
        }

        public static List<Timetracker> GetMonthlyTimetrackersForDepartment(int id)
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return TimetrackerRepository.GetTimetrackersForDepartment(id).
                Where(t => t.DateTimeStart >= firstDayOfMonth && t.DateTimeStart <= lastDayOfMonth).ToList();
        }

        public static List<Timetracker> GetTotalTimetrackersForDepartment(int id)
        {
            return TimetrackerRepository.GetTimetrackersForDepartment(id);
        }

        public static List<Timetracker> GetWeeklyTimetrackersForEmployee(int id)
        {
            var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
            {
                monday = monday.AddDays(-7);
            }
            var sunday = monday.AddDays(6);

            return TimetrackerRepository.GetTimetrackersForEmployee(id).
                Where(t => t.DateTimeStart >= monday && t.DateTimeStart <= sunday).ToList();
        }


        public static bool StopTimetracker(int timetrackerId, int employeeId)
        {
            Timetracker? timetracker = EmployeeLogic.GetEmployee(employeeId)?.Timetrackers.FirstOrDefault(t => t.Id == timetrackerId);

            if (timetracker == null || timetracker.DateTimeEnd != null)
                return false;

            timetracker.DateTimeEnd = DateTime.Now;
            TimetrackerRepository.UpdateTimetracker(timetracker);
            return true;
        }

        public static List<Timetracker> GetTimetrackers()
        {
            return TimetrackerRepository.GetTimetrackers();
        }
    }
}
