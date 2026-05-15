
using FluentValidation;
using FluentValidation.Results;
using LoginSignup.DTOs;
using LoginSignup.Services.Interfaces;
using LoginSignupBackend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginSignup.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<UserRegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public AuthController(
            IAuthService authService,
            IValidator<UserRegisterDto> registerValidator,
            IValidator<LoginDto> loginValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            ValidationResult validation = await _registerValidator.ValidateAsync(dto);

            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            //try
            //{
            //    await _authService.RegisterAsync(dto);
            //    return Ok(new { message = "User registered successfully" });
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(new { error = ex.Message });
            //}

            await _authService.RegisterAsync(dto);
            return Ok(new { message = "User registered successfully" });
        }

        // =========================
        // LOGIN (Access + Refresh Token)
        // =========================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            ValidationResult validation = await _loginValidator.ValidateAsync(dto);

            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            //try
            //{
            //    var tokens = await _authService.LoginAsync(dto);
            //    return Ok(tokens); // returns AccessToken + RefreshToken
            //}
            //catch (Exception ex)
            //{
            //    return Unauthorized(new { error = ex.Message });
            //}
            var tokens=await _authService.LoginAsync(dto);
            return Ok(tokens);
        }
        //[Authorize]
        //[HttpGet("validate")]
        //public IActionResult ValidateToken()
        //{
        //    var userId = User.FindFirst("UserId")?.Value;
        //    var email = User.FindFirst(ClaimTypes.Email)?.Value;
        //    var role = User.FindFirst(ClaimTypes.Role)?.Value;
        //    var username = User.FindFirst(ClaimTypes.Name)?.Value;
        //    var employmentId = User.FindFirst("EmploymentID")?.Value;
        //    var panCard = User.FindFirst("PanCard")?.Value;

        //    var response = new AuthUserDto
        //    {
        //        UserId = Guid.Parse(userId),
        //        Email = email,
        //        Username = username,
        //        Role = role,
        //        EmploymentID = employmentId,
        //        PanCard = panCard
        //    };

        //    return Ok(response);
        //}
        [Authorize]
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateToken([FromServices] IUserService userService)
        {
            var userId = User.FindFirst("UserId")?.Value;

            // ✅ Always fetch fresh data from DB instead of relying on token claims
            var user = await userService.GetByIdAsync(Guid.Parse(userId!));

            var response = new AuthUserDto
            {
                UserId = user.Id,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                EmploymentID = user.EmploymentID, // ✅ Always fresh from DB
                PanCard = user.PanCard            // ✅ Always fresh from DB
            };

            return Ok(response);
        }

        // =========================
        // REFRESH TOKEN
        // =========================
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            //try
            //{
            //    var tokens = await _authService.RefreshTokenAsync(dto);
            //    return Ok(tokens);
            //}
            //catch (Exception ex)
            //{
            //    return Unauthorized(new { error = ex.Message });
            //}
            var tokens=await _authService.RefreshTokenAsync(dto);
            return Ok(tokens);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { error = "Email is required" });

            await _authService.ForgotPasswordAsync(dto.Email);

            return Ok(new
            {
                message = "If account exists, OTP has been sent to email"
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Otp) ||
                string.IsNullOrWhiteSpace(dto.NewPassword) ||
                string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                return BadRequest(new { error = "All fields are required" });
            }

            await _authService.ResetPasswordAsync(dto);
            return Ok(new { message = "Password reset successful" });
        }



        // =========================
        // LOGOUT
        // =========================
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            await _authService.LogoutAsync(email);
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
