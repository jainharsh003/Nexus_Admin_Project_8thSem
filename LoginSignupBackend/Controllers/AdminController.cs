

    using Microsoft.AspNetCore.Mvc;
    using LoginSignup.DTOs;
    using LoginSignup.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LoginSignup.Controllers
    {
        [ApiController]
        [Route("api/admin")]
        [Authorize(Roles = "Admin")]
        public class AdminController : ControllerBase
        {
            private readonly IAdminService _adminService;

            public AdminController(IAdminService adminService)
            {
                _adminService = adminService;
            }

            // =========================
            // GET ALL USERS
            // =========================
            [HttpGet("users")]
            public async Task<IActionResult> GetAllUsers()
            {
                //var email = User.Identity!.Name;
                var email=User.FindFirst(ClaimTypes.Email)?.Value;  
            var users = await _adminService.GetAllUsersAsync(email);
                return Ok(users);
            }

            // =========================
            // UPDATE USER
            // =========================
            [HttpPut("users/{id}")]
            public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
            {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;


            await _adminService.UpdateUserAsync(id, dto, email);

                return Ok(new { message = "User updated successfully." });
            }

            // =========================
            // DELETE USER
            // =========================
            [HttpDelete("users/{id}")]
            public async Task<IActionResult> DeleteUser(Guid id)
            {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;


            await _adminService.DeleteUserAsync(id, email);

                return Ok(new { message = "User deleted successfully." });
            }
        }
    }


