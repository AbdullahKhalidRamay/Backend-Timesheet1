using System;

namespace TimeSheetAPI.Models.DTOs
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid? LeaderId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public DepartmentDto Department { get; set; }
        public UserDto? Leader { get; set; }
        public UserDto Creator { get; set; }
        public List<UserDto> Members { get; set; } = new List<UserDto>();
        public List<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    }

    public class TeamDetailDto : TeamDto
    {
        public int MemberCount { get; set; }
        public int ProjectCount { get; set; }
    }

    public class CreateTeamRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid? LeaderId { get; set; }
    }

    public class UpdateTeamRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? LeaderId { get; set; }
    }

    public class AddTeamMemberRequest
    {
        public Guid UserId { get; set; }
    }

    public class TeamStatistics
    {
        public int TotalMembers { get; set; }
        public int TotalProjects { get; set; }
        public decimal TotalHours { get; set; }
        public decimal BillableHours { get; set; }
        public decimal NonBillableHours { get; set; }
        public decimal AverageHoursPerMember { get; set; }
    }
}
