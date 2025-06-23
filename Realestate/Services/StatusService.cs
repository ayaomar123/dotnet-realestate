using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Realestate.Data;
using Realestate.DTOs.Category;
using Realestate.DTOs.Status;
using Realestate.Entities;
using Realestate.Interfaces;

namespace Realestate.Services
{
    public class StatusService(AppDbContext context, IWebHostEnvironment env, IMapper mapper) : IStatusService
    {


        public async Task<List<StatusResponseDto>> GetAllAsync()
        {
            var types = await context.Statuses.ToListAsync();

            return mapper.Map<List<StatusResponseDto>>(types);
        }

        public async Task<StatusResponseDto> CreateAsync(StatusRequestDto dto)
        {
            var category = mapper.Map<Status>(dto);

            if (dto.Image is not null)
            {
                category.Image = await SaveImageAsync(dto.Image);
            }

            context.Statuses.Add(category);
            await context.SaveChangesAsync();

            return mapper.Map<StatusResponseDto>(category);
        }

        public async Task<StatusResponseDto?> UpdateAsync(int id, StatusRequestDto dto)
        {
            var category = await context.Statuses.FindAsync(id); ;
            if (category == null) return null;

            mapper.Map(dto, category);
            if (dto.Image is not null)
            {
                category.Image = await SaveImageAsync(dto.Image);
            }

            await context.SaveChangesAsync();

            return mapper.Map<StatusResponseDto>(category);
        }

        public async Task<StatusResponseDto?> UpdateStatusAsync(int id)
        {
            var category = await context.Statuses.FindAsync(id); ;
            if (category == null)
                return null;
            category.IsActive = !category.IsActive;
            await context.SaveChangesAsync();

            return mapper.Map<StatusResponseDto>(category);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var status = await context.Statuses.FindAsync(id);
            if (status == null) return false;

            context.Statuses.Remove(status);
            await context.SaveChangesAsync();
            return true;
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required.");

            string uploadsPath = Path.Combine(env.WebRootPath, "images/statuses");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/statuses/{fileName}";
        }
    }
}
