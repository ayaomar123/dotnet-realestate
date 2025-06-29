

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Realestate.Data;
using Realestate.DTOs.Category;
using Realestate.Entities;
using Realestate.Interfaces;

namespace Realestate.Services
{
    public class CityService(AppDbContext context, IWebHostEnvironment env, IMapper mapper) : ICityService
    {


        public async Task<List<BaseResponseDto>> GetAllAsync()
        {
            var items = await context.Cities.ToListAsync();

            return mapper.Map<List<BaseResponseDto>>(items);
        }

        public async Task<BaseResponseDto> CreateAsync(BaseRequestDto dto)
        {
            var item = mapper.Map<City>(dto);

            if (dto.Image is not null)
            {
                item.Image = await SaveImageAsync(dto.Image);
            }

            context.Cities.Add(item);
            await context.SaveChangesAsync();

            return mapper.Map<BaseResponseDto>(item);
        }

        public async Task<BaseResponseDto?> UpdateAsync(int id, BaseRequestDto dto)
        {
            var item = await context.Cities.FindAsync(id); ;
            if (item == null) return null;

            mapper.Map(dto, item);
            if (dto.Image is not null)
            {
                item.Image = await SaveImageAsync(dto.Image);
            }

            await context.SaveChangesAsync();

            return mapper.Map<BaseResponseDto>(item);
        }

        public async Task<BaseResponseDto?> UpdateBaseAsync(int id)
        {
            var item = await context.Cities.FindAsync(id); ;
            if (item == null)
                return null;
            item.IsActive = !item.IsActive;
            await context.SaveChangesAsync();

            return mapper.Map<BaseResponseDto>(item);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var item = await context.Cities.FindAsync(id);
            if (item == null) return false;

            context.Cities.Remove(item);
            await context.SaveChangesAsync();
            return true;
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required.");

            string uploadsPath = Path.Combine(env.WebRootPath, "images/cities");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"images/cities/{fileName}";
        }
    }

}