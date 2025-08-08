using System;
using System.Collections.Generic;

namespace TimeSheetAPI.Models.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public string? RelatedEntityType { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public UserDto User { get; set; }
    }

    public class ReminderDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Type { get; set; }
    }

    public class UpdateReminderSettingsRequest
    {
        public bool EmailReminders { get; set; }
        public bool PushNotifications { get; set; }
        public TimeSpan DailyReminderTime { get; set; }
        public List<string> ReminderTypes { get; set; } = new List<string>();
    }
}
