using DAL.Repositories;
using DTO.Models;

namespace BLL
{
    public static class DepartmentLogic
    {

        public static Department CreateDepartment(string departmentName)
        {
            var department = new Department() { Name = departmentName };
            DepartmentRepository.AddDepartment(department);
            return department;
        }

        public static Department GetDepartment(int id)
        {
            return DepartmentRepository.GetDepartment(id);
        }

        public static List<Department> GetDepartments()
        {
            return DepartmentRepository.GetDepartments();
        }

        public static void AddEmployeeToDepartment(int departmentId, int employeeId)
        {
            DepartmentRepository.AddEmployeeToDepartment(departmentId, employeeId);
        }
    }
}
