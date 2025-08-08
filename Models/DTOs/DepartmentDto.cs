using System;

namespace TimeSheetAPI.Models.DTOs
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ManagerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public UserDto? Manager { get; set; }
        public List<TeamDto> Teams { get; set; } = new List<TeamDto>();
    }

    public class DepartmentDetailDto : DepartmentDto
    {
        public List<UserDto> Members { get; set; } = new List<UserDto>();
        public List<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    }

    public class CreateDepartmentRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ManagerId { get; set; }
    }

    public class UpdateDepartmentRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? ManagerId { get; set; }
    }

    public class DepartmentStatistics
    {
        public int TotalMembers { get; set; }
        public int TotalTeams { get; set; }
        public int TotalProjects { get; set; }
        public decimal TotalHours { get; set; }
        public decimal BillableHours { get; set; }
        public decimal NonBillableHours { get; set; }
    }
}
