using LoginSignup.DTOs;
using LoginSignup.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginSignup.Controllers
{
    [ApiController]
    [Route("api/user")]

    // 🔥 Allow BOTH User + Admin for all endpoints in this controller
    [Authorize(Roles = "User,Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // =========================
        // GET PROFILE
        // =========================
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;


            var user = await _userService.GetProfileAsync(email);

            return Ok(user);
        }

        // =========================
        // UPDATE PROFILE
        // =========================
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;


            await _userService.UpdateProfileAsync(dto, email);

            return Ok(new { message = "Profile updated successfully." });
        }
    }
}
