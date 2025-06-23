using Realestate.DTOs.Auth;

namespace Realestate.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
        Task<AuthResponseDto?> RefreshTokensAsync(ResfreshTokenRequestDto request);
        Task<ProfileDto?> UpdateProfileAsync(string userId, UpdateProfileDto dto);
        Task<ProfileDto?> GetProfileAsync(string userId);


    }
}
