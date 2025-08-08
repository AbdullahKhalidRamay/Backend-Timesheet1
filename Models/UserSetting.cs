using System;

namespace TimeSheetAPI.Models
{
    public class UserSetting
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Theme { get; set; } = "light";
        public string DefaultView { get; set; } = "week";
        public bool NotificationsEnabled { get; set; } = true;
        public bool EmailNotificationsEnabled { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation property
        public User User { get; set; }
    }
}