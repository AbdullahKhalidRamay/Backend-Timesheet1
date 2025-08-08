using System;

namespace TimeSheetAPI.Models.DTOs
{
    public class UserSettingsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Theme { get; set; }
        public string? DefaultView { get; set; }
        public bool NotificationsEnabled { get; set; }
        public bool EmailNotificationsEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public UserDto User { get; set; }
    }

    public class UpdateUserSettingsRequest
    {
        public string? Theme { get; set; }
        public string? DefaultView { get; set; }
        public bool? NotificationsEnabled { get; set; }
        public bool? EmailNotificationsEnabled { get; set; }
    }

    public class SystemSettingsDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateSystemSettingsRequest
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
    }

    public class TimesheetDto
    {
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<TimeEntryDto> TimeEntries { get; set; } = new List<TimeEntryDto>();
        public decimal TotalHours { get; set; }
        public decimal BillableHours { get; set; }
        public decimal NonBillableHours { get; set; }
        public int TotalEntries { get; set; }
        public int PendingEntries { get; set; }
        public int ApprovedEntries { get; set; }
        public int RejectedEntries { get; set; }
    }
}
