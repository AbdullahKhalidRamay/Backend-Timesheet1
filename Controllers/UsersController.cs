using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeSheetAPI.Models;
using TimeSheetAPI.Models.DTOs;
using TimeSheetAPI.Services;

namespace TimeSheetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "owner,manager")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                Role = u.Role,
                JobTitle = u.JobTitle,
                BillableRate = u.BillableRate ?? 0,
                AvailableHours = u.AvailableHours,
                TotalBillableHours = u.TotalBillableHours,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailDto>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Check if the user is requesting their own data or is an admin/manager
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (currentUserId != id.ToString() && currentUserRole != "owner" && currentUserRole != "manager")
            {
                return Forbid();
            }

            var userDetailDto = new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                JobTitle = user.JobTitle,
                BillableRate = user.BillableRate ?? 0,
                AvailableHours = user.AvailableHours,
                TotalBillableHours = user.TotalBillableHours,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return Ok(userDetailDto);
        }

        [HttpPost]
        [Authorize(Roles = "owner")]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = new User
                {
                    Email = request.Email,
                    Name = request.Name,
                    Role = request.Role,
                    JobTitle = request.JobTitle,
                    BillableRate = request.BillableRate,
                    AvailableHours = request.AvailableHours
                };

                var createdUser = await _userService.CreateUserAsync(user, request.Password);

                var userDto = new UserDto
                {
                    Id = createdUser.Id,
                    Email = createdUser.Email,
                    Name = createdUser.Name,
                    Role = createdUser.Role,
                    JobTitle = createdUser.JobTitle,
                    BillableRate = createdUser.BillableRate ?? 0,
                    AvailableHours = createdUser.AvailableHours,
                    TotalBillableHours = createdUser.TotalBillableHours,
                    CreatedAt = createdUser.CreatedAt,
                    UpdatedAt = createdUser.UpdatedAt
                };

                return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the user is updating their own data or is an admin
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (currentUserId != id.ToString() && currentUserRole != "owner")
            {
                return Forbid();
            }

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.Name = request.Name ?? user.Name;
            user.JobTitle = request.JobTitle ?? user.JobTitle;
            user.BillableRate = request.BillableRate ?? user.BillableRate;
            user.AvailableHours = request.AvailableHours ?? user.AvailableHours;

            // Only owner can update role
            if (currentUserRole == "owner" && request.Role != null)
            {
                user.Role = request.Role;
            }

            try
            {
                await _userService.UpdateUserAsync(user);
                
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role,
                    JobTitle = user.JobTitle,
                                    BillableRate = user.BillableRate ?? 0,
                AvailableHours = user.AvailableHours,
                    TotalBillableHours = user.TotalBillableHours,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return Ok(userDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "owner")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}/timesheet")]
        [Authorize]
        public async Task<ActionResult<TimesheetDto>> GetUserTimesheet(Guid id, DateTime startDate, DateTime endDate)
        {
            // TODO: Implement timesheet functionality
            return Ok(new TimesheetDto
            {
                UserId = id,
                StartDate = startDate,
                EndDate = endDate,
                TimeEntries = new List<TimeEntryDto>(),
                TotalHours = 0,
                BillableHours = 0,
                NonBillableHours = 0,
                TotalEntries = 0,
                PendingEntries = 0,
                ApprovedEntries = 0,
                RejectedEntries = 0
            });
        }
    }


}