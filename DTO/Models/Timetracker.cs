namespace DTO.Models
{
    public class Timetracker
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int? CaseId { get; set; }

        public int DepartmentId { get; set; }

        public DateTime DateTimeStart { get; set; }

        public DateTime? DateTimeEnd { get; set; }

        public Timetracker()
        {
        }
    }
}
