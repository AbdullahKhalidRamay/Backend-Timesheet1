using Microsoft.EntityFrameworkCore;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Data
{
    public class TimeFlowDbContext : DbContext
    {
        public TimeFlowDbContext(DbContextOptions<TimeFlowDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectLevel> ProjectLevels { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectSubtask> ProjectSubtasks { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TeamProject> TeamProjects { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ApprovalAction> ApprovalActions { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<ReportTemplate> ReportTemplates { get; set; }
        public DbSet<SavedReport> SavedReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships and constraints
            
            // Configure decimal precision and scale for User properties
            modelBuilder.Entity<User>()
                .Property(u => u.BillableRate)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<User>()
                .Property(u => u.AvailableHours)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<User>()
                .Property(u => u.TotalBillableHours)
                .HasColumnType("decimal(18,2)");
                
            // Configure decimal precision and scale for TimeEntry properties
            modelBuilder.Entity<TimeEntry>()
                .Property(te => te.ActualHours)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<TimeEntry>()
                .Property(te => te.BillableHours)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<TimeEntry>()
                .Property(te => te.TotalHours)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<TimeEntry>()
                .Property(te => te.AvailableHours)
                .HasColumnType("decimal(18,2)");
                
            // Configure decimal precision and scale for Project properties
            modelBuilder.Entity<Project>()
                .Property(p => p.BudgetHours)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<Project>()
                .Property(p => p.BillingRate)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<Project>()
                .Property(p => p.BillableHours)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<Project>()
                .Property(p => p.ActualHours)
                .HasColumnType("decimal(18,2)");
            
            // Configure composite keys for junction tables
            modelBuilder.Entity<TeamMember>().HasKey(tm => new { tm.TeamId, tm.UserId });
            modelBuilder.Entity<TeamProject>().HasKey(tp => new { tp.TeamId, tp.ProjectId });
            
            // Configure cascade delete behavior
            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectLevels)
                .WithOne()
                .HasForeignKey(pl => pl.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ProjectLevel>()
                .HasMany(pl => pl.ProjectTasks)
                .WithOne(pt => pt.ProjectLevel)
                .HasForeignKey(pt => pt.LevelId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ProjectTask>()
                .HasMany(pt => pt.ProjectSubtasks)
                .WithOne(ps => ps.Task)
                .HasForeignKey(ps => ps.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Teams)
                .WithOne(t => t.Department)
                .HasForeignKey(t => t.DepartmentId);
                
            modelBuilder.Entity<User>()
                .HasMany(u => u.TimeEntries)
                .WithOne(te => te.User)
                .HasForeignKey(te => te.UserId);
                
            modelBuilder.Entity<Project>()
                .HasMany(p => p.TimeEntries)
                .WithOne(te => te.Project)
                .HasForeignKey(te => te.ProjectId);
                
            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Team)
                .WithMany()
                .HasForeignKey(tm => tm.TeamId);
                
            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany()
                .HasForeignKey(tm => tm.UserId);
                
            modelBuilder.Entity<TeamProject>()
                .HasOne(tp => tp.Team)
                .WithMany()
                .HasForeignKey(tp => tp.TeamId);
                
            modelBuilder.Entity<TeamProject>()
                .HasOne(tp => tp.Project)
                .WithMany()
                .HasForeignKey(tp => tp.ProjectId);
                
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId);
                
            modelBuilder.Entity<ApprovalAction>()
                .HasOne(aa => aa.TimeEntry)
                .WithMany()
                .HasForeignKey(aa => aa.TimeEntryId)
                .OnDelete(DeleteBehavior.NoAction);
                
            modelBuilder.Entity<ApprovalAction>()
                .HasOne(aa => aa.RequestedBy)
                .WithMany()
                .HasForeignKey(aa => aa.RequestedById)
                .OnDelete(DeleteBehavior.NoAction);
                
            modelBuilder.Entity<UserSetting>()
                .HasOne(us => us.User)
                .WithOne()
                .HasForeignKey<UserSetting>(us => us.UserId);
                
            modelBuilder.Entity<ReportTemplate>()
                .HasOne(rt => rt.Creator)
                .WithMany()
                .HasForeignKey(rt => rt.CreatedBy);
                
            modelBuilder.Entity<SavedReport>()
                .HasOne(sr => sr.Template)
                .WithMany()
                .HasForeignKey(sr => sr.TemplateId);
                
            modelBuilder.Entity<SavedReport>()
                .HasOne(sr => sr.User)
                .WithMany()
                .HasForeignKey(sr => sr.UserId);
                
            modelBuilder.Entity<ApprovalAction>()
                .HasOne(aa => aa.ApprovedBy)
                .WithMany()
                .HasForeignKey(aa => aa.ApprovedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}