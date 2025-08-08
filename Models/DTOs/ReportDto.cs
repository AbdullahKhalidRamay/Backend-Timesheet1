using System;

namespace TimeSheetAPI.Models.DTOs
{
    public class TimesheetReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ProjectId { get; set; }
        public List<TimeEntryDto> TimeEntries { get; set; } = new List<TimeEntryDto>();
        public decimal TotalHours { get; set; }
        public decimal BillableHours { get; set; }
        public decimal NonBillableHours { get; set; }
        public int TotalEntries { get; set; }
        public int PendingEntries { get; set; }
        public int ApprovedEntries { get; set; }
        public int RejectedEntries { get; set; }
    }

    public class DepartmentReportDto
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<UserDto> Members { get; set; } = new List<UserDto>();
        public List<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
        public decimal TotalHours { get; set; }
        public decimal BillableHours { get; set; }
        public decimal NonBillableHours { get; set; }
        public int TotalTimeEntries { get; set; }
        public int ActiveMembers { get; set; }
    }

    public class TeamReportDto
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<UserDto> Members { get; set; } = new List<UserDto>();
        public List<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
        public decimal TotalHours { get; set; }
        public decimal BillableHours { get; set; }
        public decimal NonBillableHours { get; set; }
        public int TotalTimeEntries { get; set; }
        public int ActiveMembers { get; set; }
    }

    public class ProjectReportDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<UserDto> TeamMembers { get; set; } = new List<UserDto>();
        public List<TimeEntryDto> TimeEntries { get; set; } = new List<TimeEntryDto>();
        public decimal TotalHours { get; set; }
        public decimal BillableHours { get; set; }
        public decimal NonBillableHours { get; set; }
        public decimal BudgetUtilization { get; set; }
        public int TotalTimeEntries { get; set; }
        public int ActiveUsers { get; set; }
    }

    public class ReportTemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Configuration { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public UserDto Creator { get; set; }
    }

    public class SavedReportDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? TemplateId { get; set; }
        public string Parameters { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public ReportTemplateDto? Template { get; set; }
        public UserDto User { get; set; }
    }
}
