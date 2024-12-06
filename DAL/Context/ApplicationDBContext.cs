using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    internal class ApplicationDBContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Timetracker> Timetrackers { get; set; }

        public ApplicationDBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=LOCALHOST\\SQLEXPRESS; Initial Catalog=EksamensOpgave; Integrated Security=SSPI; TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Cpr = "123456-7890", Name = "John Doe" },
                new Employee { Id = 2, Cpr = "098765-4321", Name = "Jane Doe" }
            );

            // Seed data for Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "IT" },
                new Department { Id = 2, Name = "HR" },
                new Department { Id = 3, Name = "QA" }
            );

            // Many-to-many: Department <-> Employee
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithMany(e => e.Departments)
                .UsingEntity<Dictionary<string, object>>(
                    "DepartmentEmployee",
                    j => j
                        .HasOne<Employee>()
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Department>()
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasData(
                            new { DepartmentId = 1, EmployeeId = 1 },
                            new { DepartmentId = 2, EmployeeId = 1 },
                            new { DepartmentId = 2, EmployeeId = 2 }
                        );
                    }
                );

            // One-to-many: Department -> Cases
            modelBuilder.Entity<Case>()
                .HasOne<Department>()
                .WithMany(d => d.Cases)
                .HasForeignKey(c => c.DepartmentId);

            // One-to-many: Department -> Timetrackers
            modelBuilder.Entity<Timetracker>()
                .HasOne<Department>()
                .WithMany(d => d.Timetrackers)
                .HasForeignKey(t => t.DepartmentId);

            // One-to-many: Case -> Timetrackers
            modelBuilder.Entity<Timetracker>()
                .HasOne<Case>()
                .WithMany(c => c.Timetrackers)
                .HasForeignKey(t => t.CaseId);

            // One-to-many: Employee -> Timetrackers
            modelBuilder.Entity<Timetracker>()
                .HasOne<Employee>()
                .WithMany(e => e.Timetrackers)
                .HasForeignKey(t => t.EmployeeId);

            // Seed data for Cases
            modelBuilder.Entity<Case>().HasData(
                new Case { Id = 1, Title = "Network Setup", Description = "Setup the new office network", DepartmentId = 1 },
                new Case { Id = 2, Title = "Test Cases", Description = "Write test cases for the new feature", DepartmentId = 2 },
                new Case { Id = 3, Title = "Test Automation", Description = "Automate the test cases", DepartmentId = 3 },
                new Case { Id = 4, Title = "Database Migration", Description = "Migrate the database to the new server", DepartmentId = 2 }
            );

            // Seed data for Timetrackers
            modelBuilder.Entity<Timetracker>().HasData(
                new Timetracker
                {
                    Id = 1,
                    EmployeeId = 1,
                    CaseId = 1,
                    DepartmentId = 1,
                    DateTimeStart = new DateTime(2023, 12, 1, 9, 0, 0),
                    DateTimeEnd = new DateTime(2024, 12, 1, 17, 0, 0)
                },
                new Timetracker
                {
                    Id = 2,
                    EmployeeId = 2,
                    CaseId = 2,
                    DepartmentId = 2,
                    DateTimeStart = new DateTime(2024, 11, 27, 10, 0, 0),
                    DateTimeEnd = new DateTime(2024, 12, 2, 16, 0, 0)
                },
                new Timetracker
                {
                    Id = 3,
                    EmployeeId = 1,
                    CaseId = 4,
                    DepartmentId = 2,
                    DateTimeStart = new DateTime(2024, 10, 3, 8, 0, 0),
                    DateTimeEnd = new DateTime(2024, 11, 3, 16, 0, 0)
                },
                new Timetracker
                {
                    Id = 4,
                    EmployeeId = 1,
                    DepartmentId = 1,
                    DateTimeStart = new DateTime(2024, 12, 4, 9, 0, 0)
                },
                new Timetracker
                {
                    Id = 5,
                    EmployeeId = 2,
                    DepartmentId = 2,
                    DateTimeStart = new DateTime(2024, 12, 5, 10, 0, 0)
                },
                new Timetracker
                {
                    Id = 6,
                    EmployeeId = 1,
                    DepartmentId = 2,
                    DateTimeStart = new DateTime(2024, 11, 30, 8, 0, 0),
                    DateTimeEnd = new DateTime(2024, 12, 6, 16, 0, 0)
                },
                new Timetracker
                {
                    Id = 7,
                    EmployeeId = 2,
                    DepartmentId = 3,
                    DateTimeStart = new DateTime(2024, 11, 7, 9, 0, 0),
                    DateTimeEnd = new DateTime(2024, 12, 7, 17, 0, 0)
                }
            );
        }
    }
}
