using DAL.Context;
using DAL.Mappers;
using DAL.Models;

namespace DAL.Repositories
{
    public class CaseRepository
    {
        public static DTO.Models.Case AddCase(DTO.Models.Case @case)
        {
            using (var context = new ApplicationDBContext())
            {
                Case c = CaseMapper.MapToDal(@case);
                context.Cases.Add(c);

                // add the case to the department
                var department = context.Departments.Find(c.DepartmentId);
                department.Cases.Add(c);

                context.SaveChanges();

                // remap the case to get the new database id
                return CaseMapper.Map(c);
            }
        }
    }
}
