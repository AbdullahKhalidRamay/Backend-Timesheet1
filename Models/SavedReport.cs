using System;

namespace TimeSheetAPI.Models
{
    public class SavedReport
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? TemplateId { get; set; }
        public string Parameters { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public ReportTemplate Template { get; set; }
        public User User { get; set; }
    }
}