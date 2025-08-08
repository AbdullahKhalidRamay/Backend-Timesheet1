using System;

namespace TimeSheetAPI.Models.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string JobTitle { get; set; }
        public decimal BillableRate { get; set; }
        public decimal AvailableHours { get; set; }
        public decimal TotalBillableHours { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UserDetailDto : UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string JobTitle { get; set; }
        public decimal BillableRate { get; set; }
        public decimal AvailableHours { get; set; }
    }

    public class UpdateUserRequest
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? JobTitle { get; set; }
        public decimal? BillableRate { get; set; }
        public decimal? AvailableHours { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
