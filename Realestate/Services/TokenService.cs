using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Realestate.Data;
using Realestate.DTOs.Auth;
using Realestate.Entities;
using Realestate.Interfaces;

namespace Realestate.Services
{
    public class TokenService(IConfiguration _config, AppDbContext _context) : ITokenService
    {
        public string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role ?? "Admin")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<AuthResponseDto> CreateTokenResponse(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

            return new AuthResponseDto
            {
               AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<User?> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            return user;
        }
    }
}
