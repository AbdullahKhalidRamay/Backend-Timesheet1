using System;

namespace TimeSheetAPI.Models.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal BudgetHours { get; set; }
        public decimal BillingRate { get; set; }
        public decimal BillableHours { get; set; }
        public decimal ActualHours { get; set; }
        public bool IsActive { get; set; }
        public bool IsBillable { get; set; }
        public string Status { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public UserDto Creator { get; set; }
        public List<ProjectLevelDto> ProjectLevels { get; set; } = new List<ProjectLevelDto>();
    }

    public class CreateProjectRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal BudgetHours { get; set; }
        public decimal BillingRate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsBillable { get; set; } = false;
        public string Status { get; set; } = "active";
        public List<Guid> DepartmentIds { get; set; } = new List<Guid>();
        public List<Guid> TeamIds { get; set; } = new List<Guid>();
    }

    public class UpdateProjectRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ClientName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BudgetHours { get; set; }
        public decimal? BillingRate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBillable { get; set; }
        public string? Status { get; set; }
    }

    public class ProjectLevelDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public List<ProjectTaskDto> ProjectTasks { get; set; } = new List<ProjectTaskDto>();
    }

    public class ProjectTaskDto
    {
        public Guid Id { get; set; }
        public Guid LevelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public List<ProjectSubtaskDto> ProjectSubtasks { get; set; } = new List<ProjectSubtaskDto>();
    }

    public class ProjectSubtaskDto
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateProjectLevelRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateProjectLevelRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }

    public class CreateProjectTaskRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateProjectTaskRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ProjectStatistics
    {
        public decimal TotalHours { get; set; }
        public decimal BillableHours { get; set; }
        public decimal NonBillableHours { get; set; }
        public decimal BudgetUtilization { get; set; }
        public int TotalTimeEntries { get; set; }
        public int ActiveUsers { get; set; }
    }
}
