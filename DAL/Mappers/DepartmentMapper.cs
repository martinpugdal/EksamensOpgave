
using DTO.Models;

namespace DAL.Mappers
{
    internal class DepartmentMapper
    {
        public static Department Map(Models.Department d)
        {
            return new Department()
            {
                Id = d.Id,
                Name = d.Name,
                Employees = d.Employees.Select(e => EmployeeMapper.Map(e)).ToList(),
                Cases = d.Cases.Select(c => CaseMapper.Map(c)).ToList(),
                Timetrackers = d.Timetrackers.Select(t => TimetrackerMapper.Map(t)).ToList()
            };
        }

        public static Models.Department MapToDal(Department dtoDepartment)
        {
            return new Models.Department
            {
                Id = dtoDepartment.Id,
                Name = dtoDepartment.Name,
                Employees = dtoDepartment.Employees.Select(e => EmployeeMapper.MapToDal(e)).ToList(),
                Cases = dtoDepartment.Cases.Select(c => CaseMapper.MapToDal(c)).ToList(),
                Timetrackers = dtoDepartment.Timetrackers.Select(t => TimetrackerMapper.MapToDal(t)).ToList()
            };
        }
    }
}
