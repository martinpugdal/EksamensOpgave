namespace DAL.Mappers
{
    internal class CaseMapper
    {
        public static DTO.Models.Case Map(Models.Case @case)
        {
            return new DTO.Models.Case()
            {
                Id = @case.Id,
                Title = @case.Title,
                Description = @case.Description,
                DepartmentId = @case.DepartmentId,
                Timetrackers = @case.Timetrackers.Select(@case => TimetrackerMapper.Map(@case)).ToList()
            };
        }

        public static Models.Case MapToDal(DTO.Models.Case dtoCase)
        {
            return new Models.Case
            {
                Id = dtoCase.Id,
                Title = dtoCase.Title,
                Description = dtoCase.Description,
                DepartmentId = dtoCase.DepartmentId,
                Timetrackers = dtoCase.Timetrackers.Select(@case => TimetrackerMapper.MapToDal(@case)).ToList()
            };
        }
    }
}
