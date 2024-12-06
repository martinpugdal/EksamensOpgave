
namespace DAL.Mappers
{
    internal class TimetrackerMapper
    {
        public static DTO.Models.Timetracker Map(Models.Timetracker t)
        {
            return new DTO.Models.Timetracker()
            {
                Id = t.Id,
                DateTimeStart = t.DateTimeStart,
                DateTimeEnd = t.DateTimeEnd,
                EmployeeId = t.EmployeeId,
                DepartmentId = t.DepartmentId,
                CaseId = t.CaseId
            };
        }

        public static Models.Timetracker MapToDal(DTO.Models.Timetracker dtoTimetracker)
        {
            return new Models.Timetracker
            {
                Id = dtoTimetracker.Id,
                DateTimeStart = dtoTimetracker.DateTimeStart,
                DateTimeEnd = dtoTimetracker.DateTimeEnd,
                EmployeeId = dtoTimetracker.EmployeeId,
                DepartmentId = dtoTimetracker.DepartmentId,
                CaseId = dtoTimetracker.CaseId
            };
        }
    }
}
