using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Realestate.Data;
using Realestate.DTOs.Category;
using Realestate.Entities;
using Realestate.Interfaces;

namespace Realestate.Services
{
    public class TypeService(AppDbContext context, IWebHostEnvironment env, IMapper mapper) :ITypeService
    {
        

        public async Task<List<BaseResponseDto>> GetAllAsync()
        {
            var types = await context.Types.ToListAsync();

            return mapper.Map<List<BaseResponseDto>>(types);
        }

        public async Task<BaseResponseDto> CreateAsync(BaseRequestDto dto)
        {
            var category = mapper.Map<MyType>(dto);

            if (dto.Image is not null)
            {
                category.Image = await SaveImageAsync(dto.Image);
            }

            context.Types.Add(category);
            await context.SaveChangesAsync();

            return mapper.Map<BaseResponseDto>(category);
        }

        public async Task<BaseResponseDto?> UpdateAsync(int id, BaseRequestDto dto)
        {
            var category = await context.Types.FindAsync(id); ;
            if (category == null) return null;

            mapper.Map(dto, category);
            if (dto.Image is not null)
            {
                category.Image = await SaveImageAsync(dto.Image);
            }

            await context.SaveChangesAsync();

            return mapper.Map<BaseResponseDto>(category);
        }

        public async Task<BaseResponseDto?> UpdateBaseAsync(int id)
        {
            var category = await context.Types.FindAsync(id); ;
            if (category == null)
                return null;
            category.IsActive = !category.IsActive;
            await context.SaveChangesAsync();

            return mapper.Map<BaseResponseDto>(category);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await context.Types.FindAsync(id);
            if (category == null) return false;

            context.Types.Remove(category);
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

            return $"images/types/{fileName}";
        }
    }
}
