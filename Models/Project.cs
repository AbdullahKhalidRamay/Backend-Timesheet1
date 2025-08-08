using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeSheetAPI.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string ClientName { get; set; }
        
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? EndDate { get; set; }
        
        public decimal BudgetHours { get; set; } = 0;
        
        public decimal BillingRate { get; set; } = 0;
        
        public decimal BillableHours { get; set; } = 0;
        
        public decimal ActualHours { get; set; } = 0;
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        [Required]
        public bool IsBillable { get; set; } = false;
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "active";
        
        [Required]
        public Guid CreatedBy { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User Creator { get; set; }
        public virtual ICollection<ProjectLevel> ProjectLevels { get; set; } = new List<ProjectLevel>();
        public virtual ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    }
}