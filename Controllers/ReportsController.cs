using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeSheetAPI.Models;
using TimeSheetAPI.Services;

namespace TimeSheetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IUserService _userService;

        public ReportsController(IReportService reportService, IUserService userService)
        {
            _reportService = reportService;
            _userService = userService;
        }

        [HttpGet("timesheet")]
        public async Task<IActionResult> GetTimesheetReport(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate, 
            [FromQuery] Guid? userId = null)
        {
            // If userId is not provided, use the current user's ID
            if (!userId.HasValue)
            {
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (currentUserId != null)
                {
                    userId = Guid.Parse(currentUserId);
                }
            }
            else
            {
                // Check if the user is requesting their own data or is an admin/manager
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

                if (currentUserId != userId.ToString() && currentUserRole != "Admin" && currentUserRole != "Manager")
                {
                    return Forbid();
                }
            }

            var report = await _reportService.GetTimesheetReportAsync(startDate, endDate, userId);
            return Ok(report);
        }

        [HttpGet("department")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetDepartmentReport(
            [FromQuery] Guid departmentId, 
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var report = await _reportService.GetDepartmentReportAsync(departmentId, startDate, endDate);
            return Ok(report);
        }

        [HttpGet("team")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetTeamReport(
            [FromQuery] Guid teamId, 
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var report = await _reportService.GetTeamReportAsync(teamId, startDate, endDate);
            return Ok(report);
        }

        [HttpGet("project")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetProjectReport(
            [FromQuery] Guid projectId, 
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var report = await _reportService.GetProjectReportAsync(projectId, startDate, endDate);
            return Ok(report);
        }

        [HttpGet("export/timesheet")]
        public async Task<IActionResult> ExportTimesheetReport(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate, 
            [FromQuery] Guid? userId = null, 
            [FromQuery] string format = "csv")
        {
            // If userId is not provided, use the current user's ID
            if (!userId.HasValue)
            {
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (currentUserId != null)
                {
                    userId = Guid.Parse(currentUserId);
                }
            }
            else
            {
                // Check if the user is requesting their own data or is an admin/manager
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

                if (currentUserId != userId.ToString() && currentUserRole != "Admin" && currentUserRole != "Manager")
                {
                    return Forbid();
                }
            }

            var fileData = await _reportService.ExportTimesheetReportAsync(startDate, endDate, userId, format);
            string fileName = $"timesheet_report_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.csv";

            return File(fileData, "text/csv", fileName);
        }

        [HttpGet("export/department")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ExportDepartmentReport(
            [FromQuery] Guid departmentId, 
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate, 
            [FromQuery] string format = "csv")
        {
            var fileData = await _reportService.ExportDepartmentReportAsync(departmentId, startDate, endDate, format);
            string fileName = $"department_report_{departmentId}_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.csv";

            return File(fileData, "text/csv", fileName);
        }

        // Report Templates endpoints
        [HttpGet("templates")]
        public async Task<IActionResult> GetReportTemplates()
        {
            var templates = await _reportService.GetAllReportTemplatesAsync();
            return Ok(templates);
        }

        [HttpGet("templates/{id}")]
        public async Task<IActionResult> GetReportTemplate(Guid id)
        {
            var template = await _reportService.GetReportTemplateByIdAsync(id);
            if (template == null)
            {
                return NotFound();
            }
            return Ok(template);
        }

        [HttpPost("templates")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateReportTemplate([FromBody] ReportTemplate template)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            template.CreatedBy = Guid.Parse(currentUserId);
            var createdTemplate = await _reportService.CreateReportTemplateAsync(template);
            return CreatedAtAction(nameof(GetReportTemplate), new { id = createdTemplate.Id }, createdTemplate);
        }

        [HttpPut("templates/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateReportTemplate(Guid id, [FromBody] ReportTemplate template)
        {
            if (id != template.Id)
            {
                return BadRequest();
            }

            var updatedTemplate = await _reportService.UpdateReportTemplateAsync(template);
            if (updatedTemplate == null)
            {
                return NotFound();
            }

            return Ok(updatedTemplate);
        }

        [HttpDelete("templates/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteReportTemplate(Guid id)
        {
            var result = await _reportService.DeleteReportTemplateAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Saved Reports endpoints
        [HttpGet("saved")]
        public async Task<IActionResult> GetUserSavedReports()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var reports = await _reportService.GetUserSavedReportsAsync(Guid.Parse(currentUserId));
            return Ok(reports);
        }

        [HttpGet("saved/{id}")]
        public async Task<IActionResult> GetSavedReport(Guid id)
        {
            var report = await _reportService.GetSavedReportByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            // Check if the user is requesting their own data or is an admin/manager
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (currentUserId != report.UserId.ToString() && currentUserRole != "Admin" && currentUserRole != "Manager")
            {
                return Forbid();
            }

            return Ok(report);
        }

        [HttpPost("saved")]
        public async Task<IActionResult> CreateSavedReport([FromBody] SavedReport report)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            report.UserId = Guid.Parse(currentUserId);
            var createdReport = await _reportService.CreateSavedReportAsync(report);
            return CreatedAtAction(nameof(GetSavedReport), new { id = createdReport.Id }, createdReport);
        }

        [HttpPut("saved/{id}")]
        public async Task<IActionResult> UpdateSavedReport(Guid id, [FromBody] SavedReport report)
        {
            if (id != report.Id)
            {
                return BadRequest();
            }

            var existingReport = await _reportService.GetSavedReportByIdAsync(id);
            if (existingReport == null)
            {
                return NotFound();
            }

            // Check if the user is updating their own report or is an admin/manager
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (currentUserId != existingReport.UserId.ToString() && currentUserRole != "Admin" && currentUserRole != "Manager")
            {
                return Forbid();
            }

            // Preserve the original user ID
            report.UserId = existingReport.UserId;

            var updatedReport = await _reportService.UpdateSavedReportAsync(report);
            return Ok(updatedReport);
        }

        [HttpDelete("saved/{id}")]
        public async Task<IActionResult> DeleteSavedReport(Guid id)
        {
            var report = await _reportService.GetSavedReportByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            // Check if the user is deleting their own report or is an admin/manager
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (currentUserId != report.UserId.ToString() && currentUserRole != "Admin" && currentUserRole != "Manager")
            {
                return Forbid();
            }

            var result = await _reportService.DeleteSavedReportAsync(id);
            return NoContent();
        }
    }
}