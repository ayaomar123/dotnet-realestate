

using AutoMapper;
using Realestate.Data;
using Realestate.DTOs.Category;
using Realestate.Entities;
using Realestate.Interfaces;

namespace Realestate.Services
{
    public class CityService : GenericCrudBaseService<City, BaseRequestDto, BaseResponseDto>, ICityInterface
    {
        public CityService(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
            : base(context, env, mapper)
        {
        }

        public async Task<BaseResponseDto?> UpdateBaseAsync(int id)
        {
            var category = await _dbSet.FindAsync(id);
            if (category == null) return null;

            category.IsActive = !category.IsActive;
            await _context.SaveChangesAsync();
            return _mapper.Map<BaseResponseDto>(category);
        }

        protected override async Task HandleFileUploadAsync(BaseRequestDto dto, City entity)
        {
            if (dto.Image is not null)
            {
                string folder = "images/cities";
                var uploadsPath = Path.Combine(_env.WebRootPath, folder);
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var fullPath = Path.Combine(uploadsPath, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);

                entity.Image = $"/{folder}/{fileName}".Replace("\\", "/");
            }
        }
    }

}