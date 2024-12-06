using DAL.Context;
using DAL.Mappers;
using DTO.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DepartmentRepository
    {

        /*
         * Get a single department from the database
         */
        public static Department GetDepartment(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                var department = context.Departments
                     .Include(d => d.Employees)
                     .Include(d => d.Cases)
                     .Include(d => d.Timetrackers)
                     .FirstOrDefault(d => d.Id == id);

                return DepartmentMapper.Map(department);
            }
        }

        /*
         * Get all departments from the database
         */
        public static List<Department> GetDepartments()
        {
            using (var context = new ApplicationDBContext())
            {
                var departments = context.Departments
                    .Include(d => d.Employees)
                    .Include(d => d.Cases)
                    .Include(d => d.Timetrackers)
                    .ToList();
                return departments.Select(e => DepartmentMapper.Map(e)).ToList();
            }
        }

        /*
         * Add a new department to the database
         */
        public static Department AddDepartment(Department department)
        {
            using (var context = new ApplicationDBContext())
            {
                Models.Department d = DepartmentMapper.MapToDal(department);
                context.Departments.Add(d);
                context.SaveChanges();

                // remap the department to get the new database id
                return DepartmentMapper.Map(d);
            }
        }

        /*
         * Remove a department from the database
         */
        public static void RemoveDepartment(Department department)
        {
            using (var context = new ApplicationDBContext())
            {
                Models.Department d = DepartmentMapper.MapToDal(department);
                context.Departments.Remove(d);
                context.SaveChanges();
            }
        }

        /*
         * Update a department in the database
         */
        public static void UpdateDepartment(Department department)
        {
            using (var context = new ApplicationDBContext())
            {
                Models.Department d = DepartmentMapper.MapToDal(department);
                context.Departments.Update(d);

                //todo: rest idk?

                context.SaveChanges();
            }
        }

        public static void AddEmployeeToDepartment(int departmentId, int employeeId)
        {
            using (var context = new ApplicationDBContext())
            {
                var department = context.Departments
                    .Include(d => d.Employees)
                    .Include(d => d.Cases)
                    .Include(d => d.Timetrackers)
                    .FirstOrDefault(d => d.Id == departmentId);

                if (department == null)
                    return;

                var employee = context.Employees
                    .FirstOrDefault(e => e.Id == employeeId);

                if (employee == null)
                    return;

                department.Employees.Add(employee);
                employee.Departments.Add(department);
                context.SaveChanges();
            }
        }
    }
}
