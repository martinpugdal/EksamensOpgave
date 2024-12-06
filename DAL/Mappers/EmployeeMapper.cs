
namespace DAL.Mappers
{
    internal class EmployeeMapper
    {
        public static DTO.Models.Employee Map(Models.Employee e)
        {
            return new DTO.Models.Employee()
            {
                Id = e.Id,
                Cpr = e.Cpr,
                Name = e.Name,
                Departments = e.Departments?.Select(d => new DTO.Models.Department()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Employees = d.Employees?.Select(e => MapWithoutDepartment(e)).ToList(),
                    Cases = d.Cases?.Select(c => CaseMapper.Map(c)).ToList(),
                }).ToList(),
                Timetrackers = e.Timetrackers?.Select(t => TimetrackerMapper.Map(t)).ToList()
            };
        }

        public static Models.Employee MapToDal(DTO.Models.Employee dtoEmployee)
        {
            return new Models.Employee
            {
                Id = dtoEmployee.Id,
                Cpr = dtoEmployee.Cpr,
                Name = dtoEmployee.Name,
                Departments = dtoEmployee.Departments?.Select(d => new Models.Department()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Employees = d.Employees?.Select(e => MapToDalWithoutDepartment(e)).ToList(),
                    Cases = d.Cases?.Select(c => CaseMapper.MapToDal(c)).ToList()
                }).ToList(),
                Timetrackers = dtoEmployee.Timetrackers?.Select(t => TimetrackerMapper.MapToDal(t)).ToList()
            };
        }

        public static DTO.Models.Employee MapWithoutDepartment(Models.Employee e)
        {
            return new DTO.Models.Employee()
            {
                Id = e.Id,
                Cpr = e.Cpr,
                Name = e.Name,
                Timetrackers = e.Timetrackers?.Select(t => TimetrackerMapper.Map(t)).ToList()
            };
        }

        public static Models.Employee MapToDalWithoutDepartment(DTO.Models.Employee dtoEmployee)
        {
            return new Models.Employee
            {
                Id = dtoEmployee.Id,
                Cpr = dtoEmployee.Cpr,
                Name = dtoEmployee.Name,
                Timetrackers = dtoEmployee.Timetrackers?.Select(t => TimetrackerMapper.MapToDal(t)).ToList()
            };
        }
    }
}
