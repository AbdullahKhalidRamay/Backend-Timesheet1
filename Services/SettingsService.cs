using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeSheetAPI.Data;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly TimeFlowDbContext _context;

        public SettingsService(TimeFlowDbContext context)
        {
            _context = context;
        }

        // User Settings
        public async Task<UserSetting> GetUserSettingsAsync(Guid userId)
        {
            var settings = await _context.UserSettings
                .FirstOrDefaultAsync(us => us.UserId == userId);

            if (settings == null)
            {
                // Create default settings if none exist
                settings = new UserSetting
                {
                    UserId = userId,
                    Theme = "light",
                    DefaultView = "week",
                    NotificationsEnabled = true,
                    EmailNotificationsEnabled = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.UserSettings.Add(settings);
                await _context.SaveChangesAsync();
            }

            return settings;
        }

        public async Task<UserSetting> UpdateUserSettingsAsync(UserSetting settings)
        {
            var existingSettings = await _context.UserSettings
                .FirstOrDefaultAsync(us => us.UserId == settings.UserId);

            if (existingSettings == null)
            {
                return await CreateUserSettingsAsync(settings);
            }

            existingSettings.Theme = settings.Theme;
            existingSettings.DefaultView = settings.DefaultView;
            existingSettings.NotificationsEnabled = settings.NotificationsEnabled;
            existingSettings.EmailNotificationsEnabled = settings.EmailNotificationsEnabled;
            existingSettings.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingSettings;
        }

        public async Task<UserSetting> CreateUserSettingsAsync(UserSetting settings)
        {
            settings.CreatedAt = DateTime.UtcNow;
            settings.UpdatedAt = DateTime.UtcNow;

            _context.UserSettings.Add(settings);
            await _context.SaveChangesAsync();
            return settings;
        }

        // System Settings
        public async Task<IEnumerable<SystemSetting>> GetAllSystemSettingsAsync()
        {
            return await _context.SystemSettings.ToListAsync();
        }

        public async Task<SystemSetting> GetSystemSettingByKeyAsync(string key)
        {
            return await _context.SystemSettings
                .FirstOrDefaultAsync(ss => ss.Key == key);
        }

        public async Task<SystemSetting> UpdateSystemSettingAsync(SystemSetting setting)
        {
            var existingSetting = await _context.SystemSettings
                .FirstOrDefaultAsync(ss => ss.Id == setting.Id);

            if (existingSetting == null)
            {
                return null;
            }

            existingSetting.Value = setting.Value;
            existingSetting.Description = setting.Description;
            existingSetting.Type = setting.Type;
            existingSetting.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingSetting;
        }

        public async Task<SystemSetting> CreateSystemSettingAsync(SystemSetting setting)
        {
            // Check if a setting with the same key already exists
            var existingSetting = await _context.SystemSettings
                .FirstOrDefaultAsync(ss => ss.Key == setting.Key);

            if (existingSetting != null)
            {
                return null; // Key must be unique
            }

            setting.CreatedAt = DateTime.UtcNow;
            setting.UpdatedAt = DateTime.UtcNow;

            _context.SystemSettings.Add(setting);
            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<bool> DeleteSystemSettingAsync(Guid id)
        {
            var setting = await _context.SystemSettings.FindAsync(id);
            if (setting == null)
            {
                return false;
            }

            _context.SystemSettings.Remove(setting);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}