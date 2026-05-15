using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserDetails.DTOs;
using UserDetails.Services.Interface;

namespace UserDetails.Controllers
{
    [ApiController]
    [Route("api/userdetails")]
    [Authorize]
    public class UserDetailsController : ControllerBase
    {
        private readonly IUserDetailsService _service;

        public UserDetailsController(IUserDetailsService service)
        {
            _service = service;
        }

        // =========================
        // 🔵 CREATE USER DETAILS
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDetailsDto dto)
        {
            var authUser = HttpContext.Items["AuthUser"] as AuthUserDto;

            if (authUser == null)
                return Unauthorized("User not found in token");

            var result = await _service.CreateAsync(dto, authUser.UserId);

            return Ok(result);
        }

        // =========================
        // 🔵 GET FULL DETAILS (for logged-in user)
        // =========================
        [HttpGet("full-profile")]
        
        public async Task<IActionResult> GetFullProfile()
        {
            var authUser = HttpContext.Items["AuthUser"] as AuthUserDto;

            if (authUser == null)
                return Unauthorized();

            var token = HttpContext.Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", "");

            var result = await _service.GetFullUserProfile(authUser.UserId, token);

            return Ok(result);
        }
    }
}