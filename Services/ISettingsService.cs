using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Services
{
    public interface ISettingsService
    {
        // User Settings
        Task<UserSetting> GetUserSettingsAsync(Guid userId);
        Task<UserSetting> UpdateUserSettingsAsync(UserSetting settings);
        Task<UserSetting> CreateUserSettingsAsync(UserSetting settings);
        
        // System Settings
        Task<IEnumerable<SystemSetting>> GetAllSystemSettingsAsync();
        Task<SystemSetting> GetSystemSettingByKeyAsync(string key);
        Task<SystemSetting> UpdateSystemSettingAsync(SystemSetting setting);
        Task<SystemSetting> CreateSystemSettingAsync(SystemSetting setting);
        Task<bool> DeleteSystemSettingAsync(Guid id);
    }
}