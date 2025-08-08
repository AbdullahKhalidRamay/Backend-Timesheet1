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
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserSettings()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var settings = await _settingsService.GetUserSettingsAsync(Guid.Parse(currentUserId));
            return Ok(settings);
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUserSettings([FromBody] UserSetting settings)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            // Ensure the user can only update their own settings
            settings.UserId = Guid.Parse(currentUserId);

            var updatedSettings = await _settingsService.UpdateUserSettingsAsync(settings);
            return Ok(updatedSettings);
        }

        [HttpGet("system")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSystemSettings()
        {
            var settings = await _settingsService.GetAllSystemSettingsAsync();
            return Ok(settings);
        }

        [HttpGet("system/{key}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSystemSettingByKey(string key)
        {
            var setting = await _settingsService.GetSystemSettingByKeyAsync(key);
            if (setting == null)
            {
                return NotFound();
            }

            return Ok(setting);
        }

        [HttpPut("system")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSystemSetting([FromBody] SystemSetting setting)
        {
            var updatedSetting = await _settingsService.UpdateSystemSettingAsync(setting);
            if (updatedSetting == null)
            {
                return NotFound();
            }

            return Ok(updatedSetting);
        }

        [HttpPost("system")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSystemSetting([FromBody] SystemSetting setting)
        {
            var createdSetting = await _settingsService.CreateSystemSettingAsync(setting);
            if (createdSetting == null)
            {
                return BadRequest("A setting with this key already exists.");
            }

            return CreatedAtAction(nameof(GetSystemSettingByKey), new { key = createdSetting.Key }, createdSetting);
        }

        [HttpDelete("system/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSystemSetting(Guid id)
        {
            var result = await _settingsService.DeleteSystemSettingAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}