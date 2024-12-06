using DAL.Repositories;
using DTO.Models;

namespace BLL
{
    public static class EmployeeLogic
    {

        public static Employee CreateEmployee(string name, string cpr)
        {
            Employee employee = new Employee { Name = name, Cpr = cpr };
            Employee dbEmployee = EmployeeRepository.AddEmployee(employee);

            return dbEmployee;
        }

        public static Employee GetEmployee(int id)
        {
            return EmployeeRepository.GetEmployee(id);
        }

        public static List<Employee> GetEmployees()
        {
            return EmployeeRepository.GetEmployees();
        }
    }
}
