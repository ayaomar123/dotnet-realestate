using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Realestate.Data;
using Realestate.DTOs.Item;
using Realestate.Entities;
using Realestate.Interfaces;

namespace Realestate.Services
{
    public class ItemService(AppDbContext context, IWebHostEnvironment env, IMapper mapper) : IItemService
    {
        public async Task<List<ItemResponseDto>> GetAllAsync()
        {
            var items = await context.Items
                .Include(o => o.Category)
                .Include(o => o.District)
                .Include(o => o.City)
                .Include(o => o.MyType)
                .Include(o => o.PropertyType)
                .Include(o => o.Status)
                .Include(o => o.Images)
                .ToListAsync();

            return mapper.Map<List<ItemResponseDto>>(items);
        }

        public async Task<ItemResponseDto> CreateAsync(ItemRequestDto dto)
        {
            var item = mapper.Map<Item>(dto);

            if (dto.Image is not null)
            {
                item.Image = await SaveImageAsync(dto.Image);
            }

            if (dto.Images is not null)
            {
                item.Images = new List<Image>();

                foreach (var file in dto.Images)
                {
                    var imagePath = await SaveImageAsync(file);

                    item.Images.Add(new Image{ImageUrl = imagePath});
                }
            }

            context.Items.Add(item);
            await context.SaveChangesAsync();

            return mapper.Map<ItemResponseDto>(item);
        }

        public async Task<ItemResponseDto?> UpdateAsync(int id, ItemRequestDto dto)
        {
            var item = await context
                                .Items.Include(i => i.Images)
                                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return null;

            mapper.Map(dto, item);
            if (dto.Image is not null)
            {
                item.Image = await SaveImageAsync(dto.Image);
            }

            if (dto.Images is not null)
            {
                item.Images = new List<Image>();

                foreach (var file in dto.Images)
                {
                    var imagePath = await SaveImageAsync(file);

                    item.Images.Add(new Image { ImageUrl = imagePath });
                }
            }

            await context.SaveChangesAsync();

            return mapper.Map<ItemResponseDto>(item);
        }

        public async Task<ItemResponseDto?> UpdateBaseAsync(int id)
        {
            var item = await context.Items.FindAsync(id); ;
            if (item == null)
                return null;
            item.IsActive = !item.IsActive;
            await context.SaveChangesAsync();

            return mapper.Map<ItemResponseDto>(item);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var item = await context.Items.FindAsync(id);
            if (item == null) return false;

            context.Items.Remove(item);
            await context.SaveChangesAsync();
            return true;
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required.");

            string uploadsPath = Path.Combine(env.WebRootPath, "images/items");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/items/{fileName}";
        }
    }
}
