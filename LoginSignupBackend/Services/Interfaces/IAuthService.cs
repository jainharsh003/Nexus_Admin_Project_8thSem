using LoginSignup.DTOs;
using LoginSignupBackend.DTOs;

namespace LoginSignup.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(UserRegisterDto dto);
        Task<TokenResponseDto> RefreshTokenAsync(RefreshRequestDto dto);

        Task<TokenResponseDto> LoginAsync(LoginDto dto);

        //Task<string> VerifyOtpAsync(string email, int otp);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDto dto);

        Task LogoutAsync(string email);
    }
}
