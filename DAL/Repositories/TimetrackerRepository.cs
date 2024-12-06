using DAL.Context;
using DAL.Mappers;
using DTO.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TimetrackerRepository
    {
        public static Timetracker? AddTimetracker(Timetracker timetracker)
        {
            using (var context = new ApplicationDBContext())
            {
                Models.Timetracker t = TimetrackerMapper.MapToDal(timetracker);

                // add the timetracker to the department
                var department = context.Departments.Include(d => d.Timetrackers).FirstOrDefault(d => d.Id == t.DepartmentId);
                department.Timetrackers.Add(t);

                // add the timetracker to the employee
                var employee = context.Employees.Include(e => e.Timetrackers).FirstOrDefault(e => e.Id == t.EmployeeId);
                employee.Timetrackers.Add(t);

                // add the timetracker to the case
                if (t.CaseId != null)
                {
                    var @case = department?.Cases.FirstOrDefault(c => c.Id == t.CaseId);
                    @case?.Timetrackers.Add(t);
                }

                context.SaveChanges();
                return TimetrackerMapper.Map(t);
            }
        }

        public static List<Timetracker> GetTimetrackers()
        {
            using (var context = new ApplicationDBContext())
            {
                return context.Timetrackers.Select(TimetrackerMapper.Map).ToList();
            }
        }

        public static List<Timetracker> GetTimetrackersForDepartment(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                var department = context.Departments.
                    Include(d => d.Timetrackers).
                    Include(d => d.Employees).
                    FirstOrDefault(d => d.Id == id);

                if (department == null)
                    return new List<Timetracker>();

                return department.Timetrackers.Select(TimetrackerMapper.Map).ToList();
            }
        }

        public static List<Timetracker> GetTimetrackersForEmployee(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                var employee = context.Employees.
                    Include(e => e.Timetrackers).
                    FirstOrDefault(e => e.Id == id);

                if (employee == null)
                    return new List<Timetracker>();

                return employee.Timetrackers.Select(TimetrackerMapper.Map).ToList();
            }
        }

        public static void UpdateTimetracker(Timetracker timetracker)
        {
            using (var context = new ApplicationDBContext())
            {
                Models.Timetracker? t = TimetrackerMapper.MapToDal(timetracker);

                if (t == null)
                    return;


                // update department's timetracker
                var department = context.Departments.
                    Include(c => c.Cases).
                    Include(d => d.Timetrackers).
                    FirstOrDefault(d => d.Id == t.DepartmentId);
                var timetrackerToUpdate = department.Timetrackers.FirstOrDefault(tt => tt.Id == t.Id);
                timetrackerToUpdate.DateTimeEnd = t.DateTimeEnd;

                // update case's timetracker
                if (t.CaseId != null)
                {
                    var @case = department.Cases.FirstOrDefault(c => c.Id == t.CaseId);
                    timetrackerToUpdate = @case.Timetrackers.FirstOrDefault(tt => tt.Id == t.Id);
                    timetrackerToUpdate.DateTimeEnd = t.DateTimeEnd;
                }

                // update employee's timetracker
                var employee = context.Employees.
                    Include(e => e.Timetrackers).
                    FirstOrDefault(e => e.Id == t.EmployeeId);
                timetrackerToUpdate = employee.Timetrackers.FirstOrDefault(tt => tt.Id == t.Id);
                timetrackerToUpdate.DateTimeEnd = t.DateTimeEnd;


                context.SaveChanges();
            }
        }
    }
}