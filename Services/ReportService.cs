using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetAPI.Data;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Services
{
    public class ReportService : IReportService
    {
        private readonly TimeFlowDbContext _context;

        public ReportService(TimeFlowDbContext context)
        {
            _context = context;
        }

        // Timesheet Reports
        public async Task<IEnumerable<TimeEntry>> GetTimesheetReportAsync(DateTime startDate, DateTime endDate, Guid? userId = null)
        {
            var query = _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Where(te => te.Date >= startDate && te.Date <= endDate);

            if (userId.HasValue)
            {
                query = query.Where(te => te.UserId == userId.Value);
            }

            return await query.OrderBy(te => te.Date).ToListAsync();
        }

        // Department Reports
        public async Task<IEnumerable<object>> GetDepartmentReportAsync(Guid departmentId, DateTime startDate, DateTime endDate)
        {
            var department = await _context.Departments
                .Include(d => d.Teams)
                .FirstOrDefaultAsync(d => d.Id == departmentId);

            if (department == null)
            {
                return new List<object>();
            }

            // Get all team IDs in the department
            var teamIds = department.Teams.Select(t => t.Id).ToList();
            
            // Get all users in these teams through TeamMembers
            var userIds = await _context.TeamMembers
                .Where(tm => teamIds.Contains(tm.TeamId))
                .Select(tm => tm.UserId)
                .Distinct()
                .ToListAsync();

            // Get time entries for these users in the date range
            var timeEntries = await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Where(te => userIds.Contains(te.UserId) && te.Date >= startDate && te.Date <= endDate)
                .ToListAsync();

            // Group by user and project
            var report = timeEntries
                .GroupBy(te => new { te.UserId, UserName = te.User.Name })
                .Select(g => new
                {
                    UserId = g.Key.UserId,
                    UserName = g.Key.UserName,
                    TotalHours = g.Sum(te => te.TotalHours),
                    BillableHours = g.Sum(te => te.BillableHours),
                    Projects = g.GroupBy(te => new { te.ProjectId, ProjectName = te.Project.Name })
                        .Select(pg => new
                        {
                            ProjectId = pg.Key.ProjectId,
                            ProjectName = pg.Key.ProjectName,
                            TotalHours = pg.Sum(te => te.TotalHours),
                            BillableHours = pg.Sum(te => te.BillableHours)
                        })
                        .ToList()
                })
                .ToList();

            return report;
        }

        // Team Reports
        public async Task<IEnumerable<object>> GetTeamReportAsync(Guid teamId, DateTime startDate, DateTime endDate)
        {
            var team = await _context.Teams
                .FirstOrDefaultAsync(t => t.Id == teamId);

            if (team == null)
            {
                return new List<object>();
            }

            // Get all users in the team through TeamMembers
            var userIds = await _context.TeamMembers
                .Where(tm => tm.TeamId == teamId)
                .Select(tm => tm.UserId)
                .ToListAsync();

            // Get time entries for these users in the date range
            var timeEntries = await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Project)
                .Where(te => userIds.Contains(te.UserId) && te.Date >= startDate && te.Date <= endDate)
                .ToListAsync();

            // Group by user and project
            var report = timeEntries
                .GroupBy(te => new { te.UserId, UserName = te.User.Name })
                .Select(g => new
                {
                    UserId = g.Key.UserId,
                    UserName = g.Key.UserName,
                    TotalHours = g.Sum(te => te.TotalHours),
                    BillableHours = g.Sum(te => te.BillableHours),
                    Projects = g.GroupBy(te => new { te.ProjectId, ProjectName = te.Project.Name })
                        .Select(pg => new
                        {
                            ProjectId = pg.Key.ProjectId,
                            ProjectName = pg.Key.ProjectName,
                            TotalHours = pg.Sum(te => te.TotalHours),
                            BillableHours = pg.Sum(te => te.BillableHours)
                        })
                        .ToList()
                })
                .ToList();

            return report;
        }

        // Project Reports
        public async Task<IEnumerable<object>> GetProjectReportAsync(Guid projectId, DateTime startDate, DateTime endDate)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return new List<object>();
            }

            // Get time entries for this project in the date range
            var timeEntries = await _context.TimeEntries
                .Include(te => te.User)
                .Where(te => te.ProjectId == projectId && te.Date >= startDate && te.Date <= endDate)
                .ToListAsync();

            // Group by user
            var report = timeEntries
                .GroupBy(te => new { te.UserId, UserName = te.User.Name })
                .Select(g => new
                {
                    UserId = g.Key.UserId,
                    UserName = g.Key.UserName,
                    TotalHours = g.Sum(te => te.TotalHours),
                    BillableHours = g.Sum(te => te.BillableHours),
                    Entries = g.OrderBy(te => te.Date)
                        .Select(te => new
                        {
                            Date = te.Date,
                            TotalHours = te.TotalHours,
                            BillableHours = te.BillableHours,
                            Task = te.Task,
                            Description = te.Task
                        })
                        .ToList()
                })
                .ToList();

            return report;
        }

        // Report Templates
        public async Task<IEnumerable<ReportTemplate>> GetAllReportTemplatesAsync()
        {
            return await _context.ReportTemplates
                .Include(rt => rt.Creator)
                .ToListAsync();
        }

        public async Task<ReportTemplate> GetReportTemplateByIdAsync(Guid id)
        {
            return await _context.ReportTemplates
                .Include(rt => rt.Creator)
                .FirstOrDefaultAsync(rt => rt.Id == id);
        }

        public async Task<ReportTemplate> CreateReportTemplateAsync(ReportTemplate template)
        {
            template.CreatedAt = DateTime.UtcNow;
            template.UpdatedAt = DateTime.UtcNow;

            _context.ReportTemplates.Add(template);
            await _context.SaveChangesAsync();
            return template;
        }

        public async Task<ReportTemplate> UpdateReportTemplateAsync(ReportTemplate template)
        {
            var existingTemplate = await _context.ReportTemplates
                .FirstOrDefaultAsync(rt => rt.Id == template.Id);

            if (existingTemplate == null)
            {
                return null;
            }

            existingTemplate.Name = template.Name;
            existingTemplate.Description = template.Description;
            existingTemplate.Type = template.Type;
            existingTemplate.Configuration = template.Configuration;
            existingTemplate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingTemplate;
        }

        public async Task<bool> DeleteReportTemplateAsync(Guid id)
        {
            var template = await _context.ReportTemplates.FindAsync(id);
            if (template == null)
            {
                return false;
            }

            _context.ReportTemplates.Remove(template);
            await _context.SaveChangesAsync();
            return true;
        }

        // Saved Reports
        public async Task<IEnumerable<SavedReport>> GetUserSavedReportsAsync(Guid userId)
        {
            return await _context.SavedReports
                .Include(sr => sr.Template)
                .Where(sr => sr.UserId == userId)
                .ToListAsync();
        }

        public async Task<SavedReport> GetSavedReportByIdAsync(Guid id)
        {
            return await _context.SavedReports
                .Include(sr => sr.Template)
                .Include(sr => sr.User)
                .FirstOrDefaultAsync(sr => sr.Id == id);
        }

        public async Task<SavedReport> CreateSavedReportAsync(SavedReport report)
        {
            report.CreatedAt = DateTime.UtcNow;
            report.UpdatedAt = DateTime.UtcNow;

            _context.SavedReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<SavedReport> UpdateSavedReportAsync(SavedReport report)
        {
            var existingReport = await _context.SavedReports
                .FirstOrDefaultAsync(sr => sr.Id == report.Id);

            if (existingReport == null)
            {
                return null;
            }

            existingReport.Name = report.Name;
            existingReport.Description = report.Description;
            existingReport.TemplateId = report.TemplateId;
            existingReport.Parameters = report.Parameters;
            existingReport.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingReport;
        }

        public async Task<bool> DeleteSavedReportAsync(Guid id)
        {
            var report = await _context.SavedReports.FindAsync(id);
            if (report == null)
            {
                return false;
            }

            _context.SavedReports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        // Export Reports
        public async Task<byte[]> ExportTimesheetReportAsync(DateTime startDate, DateTime endDate, Guid? userId = null, string format = "csv")
        {
            var timeEntries = await GetTimesheetReportAsync(startDate, endDate, userId);

            if (format.ToLower() == "csv")
            {
                return GenerateTimesheetCsv(timeEntries);
            }
            else
            {
                // Default to CSV if format not supported
                return GenerateTimesheetCsv(timeEntries);
            }
        }

        public async Task<byte[]> ExportDepartmentReportAsync(Guid departmentId, DateTime startDate, DateTime endDate, string format = "csv")
        {
            var departmentReport = await GetDepartmentReportAsync(departmentId, startDate, endDate);

            if (format.ToLower() == "csv")
            {
                return GenerateDepartmentCsv(departmentReport);
            }
            else
            {
                // Default to CSV if format not supported
                return GenerateDepartmentCsv(departmentReport);
            }
        }

        // Helper methods for generating CSV files
        private byte[] GenerateTimesheetCsv(IEnumerable<TimeEntry> timeEntries)
        {
            var csv = new StringBuilder();

            // Add header
            csv.AppendLine("Date,User,Project,Task,Total Hours,Billable Hours,Description");

            // Add data rows
            foreach (var entry in timeEntries)
            {
                csv.AppendLine($"{entry.Date:yyyy-MM-dd},{entry.User.Name},{entry.Project.Name},{entry.Task},{entry.TotalHours},{entry.BillableHours},\"{EscapeCsvField(entry.Task)}\"");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        private byte[] GenerateDepartmentCsv(IEnumerable<object> departmentReport)
        {
            var csv = new StringBuilder();

            // Add header
            csv.AppendLine("User,Total Hours,Billable Hours,Project,Project Total Hours,Project Billable Hours");

            // Add data rows
            foreach (var userReport in departmentReport)
            {
                var user = (dynamic)userReport;
                bool firstUserRow = true;

                foreach (var project in user.Projects)
                {
                    if (firstUserRow)
                    {
                        csv.AppendLine($"{user.UserName},{user.TotalHours},{user.BillableHours},{project.ProjectName},{project.TotalHours},{project.BillableHours}");
                        firstUserRow = false;
                    }
                    else
                    {
                        csv.AppendLine($",,,,{project.ProjectName},{project.TotalHours},{project.BillableHours}");
                    }
                }

                // If user has no projects, still add the user row
                if (firstUserRow)
                {
                    csv.AppendLine($"{user.UserName},{user.TotalHours},{user.BillableHours},,,,");
                }
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return string.Empty;
            }

            // Replace double quotes with two double quotes
            return field.Replace("\"", "\"\"");
        }
    }
}