using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserDetails.DTOs;
using UserDetails.Services.Interface;

namespace UserDetails.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmploymentController : ControllerBase
    {
        private readonly IEmploymentService _service;

        public EmploymentController(IEmploymentService service)
        {
            _service = service;
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateEmploymentDto dto)
        {
            var user = HttpContext.Items["AuthUser"] as AuthUserDto;
            if (user == null) return Unauthorized();

            var token = HttpContext.Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", ""); // ✅ Extract token

            await _service.CreateEmployment(dto, user.UserId, token); // ✅ Pass token
            return Ok("Employment created");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateEmploymentDto dto)
        {
            var user = HttpContext.Items["AuthUser"] as AuthUserDto;
            if (user == null) return Unauthorized();

            var token = HttpContext.Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", ""); // ✅ Extract token

            await _service.UpdateEmployment(user.UserId, dto, token); // ✅ Pass token
            return Ok("Employment updated");
        }

    }
}
