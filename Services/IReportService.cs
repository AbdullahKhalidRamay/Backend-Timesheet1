using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Services
{
    public interface IReportService
    {
        // Timesheet Reports
        Task<IEnumerable<TimeEntry>> GetTimesheetReportAsync(DateTime startDate, DateTime endDate, Guid? userId = null);
        
        // Department Reports
        Task<IEnumerable<object>> GetDepartmentReportAsync(Guid departmentId, DateTime startDate, DateTime endDate);
        
        // Team Reports
        Task<IEnumerable<object>> GetTeamReportAsync(Guid teamId, DateTime startDate, DateTime endDate);
        
        // Project Reports
        Task<IEnumerable<object>> GetProjectReportAsync(Guid projectId, DateTime startDate, DateTime endDate);
        
        // Report Templates
        Task<IEnumerable<ReportTemplate>> GetAllReportTemplatesAsync();
        Task<ReportTemplate> GetReportTemplateByIdAsync(Guid id);
        Task<ReportTemplate> CreateReportTemplateAsync(ReportTemplate template);
        Task<ReportTemplate> UpdateReportTemplateAsync(ReportTemplate template);
        Task<bool> DeleteReportTemplateAsync(Guid id);
        
        // Saved Reports
        Task<IEnumerable<SavedReport>> GetUserSavedReportsAsync(Guid userId);
        Task<SavedReport> GetSavedReportByIdAsync(Guid id);
        Task<SavedReport> CreateSavedReportAsync(SavedReport report);
        Task<SavedReport> UpdateSavedReportAsync(SavedReport report);
        Task<bool> DeleteSavedReportAsync(Guid id);
        
        // Export Reports
        Task<byte[]> ExportTimesheetReportAsync(DateTime startDate, DateTime endDate, Guid? userId = null, string format = "csv");
        Task<byte[]> ExportDepartmentReportAsync(Guid departmentId, DateTime startDate, DateTime endDate, string format = "csv");
    }
}