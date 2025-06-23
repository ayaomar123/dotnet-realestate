using Realestate.DTOs.Auth;
using Realestate.Entities;

namespace Realestate.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        Task<string> GenerateAndSaveRefreshTokenAsync(User user);
        Task<AuthResponseDto> CreateTokenResponse(User user);
        Task<User?> ValidateRefreshTokenAsync(string userId, string refreshToken);
    }
}
