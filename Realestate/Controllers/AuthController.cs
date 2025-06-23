using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Realestate.DTOs;
using Realestate.DTOs.Auth;
using Realestate.Interfaces;
using Realestate.Services;

namespace Realestate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user == null)
                return BadRequest(ApiResponse<string>.Fail("User already exists or registration failed."));
            return Ok(ApiResponse<AuthResponseDto>.Ok(null, "User Registered"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto request)
        {
            var result = await authService.LoginAsync(request);
            if (result is null)
                return BadRequest(ApiResponse<string>.Fail("Invalid credentials."));

            return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Login successful."));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(ResfreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized(ApiResponse<string>.Fail("Invalid refresh token."));
            return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Token Refreshed"));
        }


        [HttpPut("profile")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await authService.UpdateProfileAsync(userId!, dto);

            return profile is not null
                ? Ok(ApiResponse<ProfileDto>.Ok(profile, "Profile updated."))
                : NotFound(ApiResponse<string>.Fail("User not found."));
        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await authService.GetProfileAsync(userId!);

            return profile is not null
                ? Ok(ApiResponse<ProfileDto>.Ok(profile))
                : NotFound(ApiResponse<string>.Fail("User not found."));
        }
    }
}
