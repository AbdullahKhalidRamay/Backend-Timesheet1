using System;

namespace TimeSheetAPI.Models.DTOs
{
    public class TimeEntryDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? ClockIn { get; set; }
        public TimeSpan? ClockOut { get; set; }
        public int? BreakTime { get; set; }
        public decimal ActualHours { get; set; }
        public decimal BillableHours { get; set; }
        public decimal TotalHours { get; set; }
        public decimal AvailableHours { get; set; }
        public string Task { get; set; }
        public string Status { get; set; }
        public bool IsBillable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public UserDto User { get; set; }
        public ProjectDto Project { get; set; }
    }

    public class CreateTimeEntryRequest
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? ClockIn { get; set; }
        public TimeSpan? ClockOut { get; set; }
        public int BreakTime { get; set; } = 0;
        public decimal ActualHours { get; set; }
        public decimal AvailableHours { get; set; } = 8;
        public string Task { get; set; }
        public bool IsBillable { get; set; } = true;
    }

    public class UpdateTimeEntryRequest
    {
        public Guid? ProjectId { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? ClockIn { get; set; }
        public TimeSpan? ClockOut { get; set; }
        public int? BreakTime { get; set; }
        public decimal? ActualHours { get; set; }
        public decimal? AvailableHours { get; set; }
        public string? Task { get; set; }
        public bool? IsBillable { get; set; }
        public string? Status { get; set; }
    }

    public class CreateBulkTimeEntriesRequest
    {
        public List<CreateTimeEntryRequest> Entries { get; set; } = new List<CreateTimeEntryRequest>();
    }

    public class TimeEntryStatusResponse
    {
        public bool HasEntries { get; set; }
        public int EntriesCount { get; set; }
        public decimal TotalHours { get; set; }
        public List<string> Statuses { get; set; } = new List<string>();
    }

    public class ApprovalRequest
    {
        public string? Comments { get; set; }
    }

    public class RejectionRequest
    {
        public string? Comments { get; set; }
    }

    public class BulkApprovalRequest
    {
        public List<Guid> TimeEntryIds { get; set; } = new List<Guid>();
        public string? Comments { get; set; }
    }

    public class TimeEntryStatistics
    {
        public decimal TotalHours { get; set; }
        public decimal BillableHours { get; set; }
        public decimal NonBillableHours { get; set; }
        public int TotalEntries { get; set; }
        public int PendingEntries { get; set; }
        public int ApprovedEntries { get; set; }
        public int RejectedEntries { get; set; }
    }
}
