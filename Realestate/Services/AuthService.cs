using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Realestate.Data;
using Realestate.DTOs.Auth;
using Realestate.Entities;
using Realestate.Interfaces;
using static System.Net.WebRequestMethods;

namespace Realestate.Services;

public class AuthService(ITokenService tokenService,AppDbContext context,
    IConfiguration config, IMapper _mapper, IHttpContextAccessor _http) : IAuthService
{
    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Username);
        if (user == null)
            return null;

        var passwordHasher = new PasswordHasher<User>();
        var isPasswordValid = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password)
                              == PasswordVerificationResult.Success;

        if (!isPasswordValid) return null;

        return await tokenService.CreateTokenResponse(user);
    }


    public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request)
    {
        if (await context.Users.AnyAsync(u => u.Email == request.Email))
            return null;

        var user = _mapper.Map<User>(request);
        user.UserName = request.Email;
        user.PasswordHash = new PasswordHasher<User>().HashPassword(null!, request.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return await tokenService.CreateTokenResponse(user);
    }


    public async Task<AuthResponseDto?> RefreshTokensAsync(ResfreshTokenRequestDto request)
    {
        var user = await tokenService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        return user != null ? await tokenService.CreateTokenResponse(user) : null;
    }

    public async Task<ProfileDto?> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null) return null;

        if (!string.IsNullOrWhiteSpace(dto.FNameAr)) user.FNameAr = dto.FNameAr;
        if (!string.IsNullOrWhiteSpace(dto.FNameEn)) user.FNameEn = dto.FNameEn;
        if (!string.IsNullOrWhiteSpace(dto.LNameAr)) user.LNameAr = dto.LNameAr;
        if (!string.IsNullOrWhiteSpace(dto.LNameEn)) user.LNameEn = dto.LNameEn;

        if (dto.Image is not null && dto.Image.Length > 0)
        {
            // حذف الصورة القديمة إن وجدت
            if (!string.IsNullOrEmpty(user.Image))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.Image.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }

            // حفظ الصورة الجديدة
            var fileExt = Path.GetExtension(dto.Image.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExt}";
            var folderPath = Path.Combine("wwwroot", "images", "users");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.Image.CopyToAsync(stream);

            user.Image = $"/images/users/{fileName}";
        }


        await context.SaveChangesAsync();

        // تجهيز DTO للإرجاع
        var result = new ProfileDto
        {
            FNameAr = user.FNameAr!,
            FNameEn = user.FNameEn!,
            LNameAr = user.LNameAr!,
            LNameEn = user.LNameEn!,
            Email = user.Email!,
            ImageUrl = string.IsNullOrEmpty(user.Image)
                ? null
                : $"{_http.HttpContext!.Request.Scheme}://{_http.HttpContext.Request.Host}{user.Image}"
        };

        return result;
    }


    public async Task<ProfileDto?> GetProfileAsync(string userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null) return null;

        var dto = new ProfileDto
        {
            FNameAr = user.FNameAr!,
            FNameEn = user.FNameEn!,
            LNameAr = user.LNameAr!,
            LNameEn = user.LNameEn!,
            Email = user.Email!
        };

        if (!string.IsNullOrEmpty(user.Image))
        {
            var baseUrl = $"{_http.HttpContext!.Request.Scheme}://{_http.HttpContext.Request.Host}";
            dto.ImageUrl = $"{baseUrl}{user.Image}";
        }

        return dto;
    }


}
