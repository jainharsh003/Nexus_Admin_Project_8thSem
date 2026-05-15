using LoginSignup.DTOs;
using LoginSignup.Helpers;
using LoginSignup.Models;
using LoginSignup.Repositories.Interfaces;
using LoginSignup.Services.Interfaces;
using LoginSignupBackend.DTOs;
using LoginSignupBackend.Services.Interfaces;

namespace LoginSignup.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;

        public AuthService(
            IUserRepository userRepo,
            IConfiguration config,
            IEmailSender emailSender)
        {
            _userRepo = userRepo;
            _config = config;
            _emailSender = emailSender;
        }

  
        public async Task RegisterAsync(UserRegisterDto dto)
        {
            var existing = await _userRepo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("Email already registered.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = PasswordHasher.Hash(dto.Password),
                IsActive = true
            };

            await _userRepo.AddAsync(user);
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid Email or Password.");

            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            var accessToken = JwtTokenGenerator.Generate(
                user,
                jwtKey,
                jwtIssuer,
                jwtAudience,
                15
            );

            var refreshToken = Guid.NewGuid().ToString();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user);

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        
        public async Task<TokenResponseDto> RefreshTokenAsync(RefreshRequestDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            if (user == null ||
                user.RefreshToken != dto.RefreshToken ||
                user.RefreshTokenExpiry <= DateTime.UtcNow)
                throw new Exception("Invalid refresh token.");

            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            var newAccessToken = JwtTokenGenerator.Generate(
                user,
                jwtKey,
                jwtIssuer,
                jwtAudience,
                15
            );

            var newRefreshToken = Guid.NewGuid().ToString();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user);

            return new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        // =========================
        // FORGOT PASSWORD (SEND OTP)
        // =========================
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email);

            if (user == null)
                return; // don't expose user existence

            var otp = new Random().Next(100000, 999999).ToString();

            user.PasswordResetOtp = otp;
            user.PasswordResetOtpExpiry = DateTime.UtcNow.AddMinutes(10);

            await _userRepo.UpdateAsync(user);

            // send email
            await _emailSender.SendOtpEmailAsync(email, otp);
        }

        // =========================
        // RESET PASSWORD
        // =========================
        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            if (user == null)
                throw new Exception("Invalid request");

            if (user.PasswordResetOtp != dto.Otp ||
                user.PasswordResetOtpExpiry < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired OTP");
            }

            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            user.PasswordHash = PasswordHasher.Hash(dto.NewPassword);

            user.PasswordResetOtp = null;
            user.PasswordResetOtpExpiry = null;

            await _userRepo.UpdateAsync(user);
        }

        // =========================
        // LOGOUT
        // =========================
        public async Task LogoutAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email);

            if (user == null)
                throw new Exception("User not found.");

            user.IsActive = false;

            await _userRepo.UpdateAsync(user);
        }
    }
}
