namespace DAL.Models
{
    internal class Employee
    {
        public int Id { get; set; }

        public string Cpr { get; set; }


        public string Name { get; set; }
        public List<Department> Departments { get; set; } = new List<Department>();

        public List<Timetracker> Timetrackers { get; set; } = new List<Timetracker>();

        public Employee()
        {
        }

        public Employee(int employeeId, string employeeCpr, string employeeName)
        {
            Id = employeeId;
            Cpr = employeeCpr;
            Name = employeeName;
        }
    }
}
