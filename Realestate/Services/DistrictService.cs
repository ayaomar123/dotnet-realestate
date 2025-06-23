using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Realestate.Data;
using Realestate.DTOs.District;
using Realestate.Entities;
using Realestate.Interfaces;

namespace Realestate.Services
{
    public class DistrictService(AppDbContext context, IWebHostEnvironment env, IMapper mapper) :IDistrictService
    {
        

        public async Task<List<DistrictResponseDto>> GetAllAsync()
        {
            var types = await context.Districts.Include(o => o.City).ToListAsync();

            return mapper.Map<List<DistrictResponseDto>>(types);
        }

        public async Task<DistrictResponseDto> CreateAsync(DistrictRequestDto dto)
        {
            var category = mapper.Map<District>(dto);

            if (dto.Image is not null)
            {
                category.Image = await SaveImageAsync(dto.Image);
            }

            context.Districts.Add(category);
            await context.SaveChangesAsync();

            return mapper.Map<DistrictResponseDto>(category);
        }

        public async Task<DistrictResponseDto?> UpdateAsync(int id, DistrictRequestDto dto)
        {
            var category = await context.Districts.FindAsync(id); ;
            if (category == null) return null;

            mapper.Map(dto, category);
            if (dto.Image is not null)
            {
                category.Image = await SaveImageAsync(dto.Image);
            }

            await context.SaveChangesAsync();

            return mapper.Map<DistrictResponseDto>(category);
        }

        public async Task<DistrictResponseDto?> UpdateStatusAsync(int id)
        {
            var category = await context.Districts.FindAsync(id); ;
            if (category == null)
                return null;
            category.IsActive = !category.IsActive;
            await context.SaveChangesAsync();

            return mapper.Map<DistrictResponseDto>(category);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await context.Districts.FindAsync(id);
            if (category == null) return false;

            context.Districts.Remove(category);
            await context.SaveChangesAsync();
            return true;
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required.");

            string uploadsPath = Path.Combine(env.WebRootPath, "images/types");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/categories/{fileName}";
        }
    }
}
