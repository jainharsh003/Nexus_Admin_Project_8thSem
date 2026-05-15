using LoginSignup.DTOs;
using LoginSignup.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSignup.Controllers
{
    [ApiController]
    [Route("api/internal/users")]
    [Authorize] // only internal services with token
    public class InternalUsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public InternalUsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);

            return Ok(new
            {
                UserId = user.Id,                 // ✅ PascalCase
                Email = user.Email,               // ✅ PascalCase
                Username = user.Username,         // ✅ PascalCase
                Role = user.Role,                 // ✅ PascalCase
                EmploymentID = user.EmploymentID, // ✅ PascalCase
                PanCard = user.PanCard            // ✅ PascalCase
            });
        }
        [HttpPut("{userId}/employment")]
        public async Task<IActionResult> UpdateEmployment(Guid userId, [FromBody] UpdateEmploymentDto dto)
        {
            await _userService.UpdateEmploymentAsync(userId, dto.EmploymentID, dto.PanCard);
            return Ok(new { message = "Employment updated" });
        }
    }
}