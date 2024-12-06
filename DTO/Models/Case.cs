﻿namespace DTO.Models
{
    public class Case
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public int DepartmentId { get; set; }

        public List<Timetracker> Timetrackers { get; set; } = new List<Timetracker>();

        public Case()
        {
        }

        public Case(int caseId, string caseTitle, string caseDescription)
        {
            Id = caseId;
            Title = caseTitle;
            Description = caseDescription;
        }
    }
}
