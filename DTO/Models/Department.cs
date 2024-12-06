namespace DTO.Models
{
    public class Department
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();

        public List<Case> Cases { get; set; } = new List<Case>();

        public List<Timetracker> Timetrackers { get; set; } = new List<Timetracker>();

        public Department()
        {
        }

        public Department(int departmentId, string departmentName)
        {
            Id = departmentId;
            Name = departmentName;
        }
    }
}
