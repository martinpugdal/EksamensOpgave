using DAL.Context;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class EmployeeRepository
    {

        /*
         * Get a single employee from the database
         */
        public static DTO.Models.Employee? GetEmployee(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                var employee = context.Employees
                     .Include(e => e.Departments)
                     .Include(e => e.Timetrackers)
                     .FirstOrDefault(e => e.Id == id);

                if (employee == null)
                    return null;

                return EmployeeMapper.Map(employee);
            }
        }

        /*
         * Get all employees from the database
         */
        public static List<DTO.Models.Employee> GetEmployees()
        {
            using (var context = new ApplicationDBContext())
            {
                var employees = context.Employees
                    .Include(e => e.Departments)
                    .Include(e => e.Timetrackers)
                    .ToList();
                return employees.Select(e => EmployeeMapper.Map(e)).ToList();
            }
        }

        /*
         * Add a new employee to the database
         */
        public static DTO.Models.Employee AddEmployee(DTO.Models.Employee employee)
        {
            using (var context = new ApplicationDBContext())
            {
                Models.Employee e = EmployeeMapper.MapToDal(employee);
                context.Employees.Add(e);
                context.SaveChanges();

                // remap the employee to get the new database id
                return EmployeeMapper.Map(e);
            }
        }

        /*
         * Remove an employee from the database
         */
        public static void RemoveEmployee(DTO.Models.Employee employee)
        {
            using (var context = new ApplicationDBContext())
            {
                Models.Employee e = EmployeeMapper.MapToDal(employee);
                context.Employees.Remove(e);
                context.SaveChanges();
            }
        }

        /*
         * Update an employee in the database
         */
        public static void UpdateEmployee(DTO.Models.Employee employee)
        {
            using (var context = new ApplicationDBContext())
            {
                var eDb = context.Employees.Include(e => e.Timetrackers).FirstOrDefault(emp => emp.Id == employee.Id);
                if (eDb != null)
                {
                    eDb.Cpr = employee.Cpr;
                    eDb.Name = employee.Name;
                    eDb.Departments = employee.Departments?.
                        Select(d => context.Departments.FirstOrDefault(department => department.Id == d.Id)).
                        ToList();

                    // get all time trackers that are already defined in the database
                    var existingTimetrackerIds = employee.Timetrackers.Select(t => t.Id).ToHashSet();

                    eDb.Timetrackers = context.Employees
                        .Include(e => e.Timetrackers)
                        .FirstOrDefault(emp => emp.Id == employee.Id)
                        .Timetrackers
                        .Where(t => existingTimetrackerIds.Contains(t.Id))
                        .ToList();

                    context.SaveChanges();
                }
            }
        }
    }
}
