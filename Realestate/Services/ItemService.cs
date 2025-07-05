using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Realestate.Data;
using Realestate.DTOs.Item;
using Realestate.DTOs.Paginations;
using Realestate.Entities;
using Realestate.Interfaces;

namespace Realestate.Services
{
    public class ItemService(AppDbContext context, IWebHostEnvironment env, IMapper mapper) : IItemService
    {
        public async Task<PaginatedResponse<ItemResponseDto>> GetAllAsync(ItemFilterDto filter)
        {
            var query = context.Items
                .Include(o => o.Category)
                .Include(o => o.District)
                .Include(o => o.City)
                .Include(o => o.MyType)
                .Include(o => o.PropertyType)
                .Include(o => o.Status)
                .Include(o => o.Images)
                .AsQueryable();

            // Apply filters
            if (filter.CityId.HasValue)
                query = query.Where(i => i.CityId == filter.CityId.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(i => i.CategoryId == filter.CategoryId.Value);

            if (filter.DistrictId.HasValue)
                query = query.Where(i => i.DistrictId == filter.DistrictId.Value);

            if (filter.PropertyTypeId.HasValue)
                query = query.Where(i => i.PropertyTypeId == filter.PropertyTypeId.Value);

            if (filter.StatusId.HasValue)
                query = query.Where(i => i.StatusId == filter.StatusId.Value);

            if (filter.AdNo.HasValue)
                query = query.Where(i => i.AdNo == filter.AdNo.Value);

            if (filter.AdvertiseNo.HasValue)
                query = query.Where(i => i.AdvertiseNo == filter.AdvertiseNo.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(i => i.IsActive == filter.IsActive.Value);

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
                query = query.Where(i =>
                    i.NameEn!.Contains(filter.Keyword) ||
                    i.NameAr!.Contains(filter.Keyword) ||
                    i.AdNo.ToString().Contains(filter.Keyword) ||
                    i.AdvertiseNo.ToString().Contains(filter.Keyword) ||
                    i.DescriptionEn!.Contains(filter.Keyword) ||
                    i.DescriptionAr!.Contains(filter.Keyword));

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var mapped = mapper.Map<List<ItemResponseDto>>(items);

            return new PaginatedResponse<ItemResponseDto>(mapped, totalCount, filter.PageNumber, filter.PageSize);
        }


        public async Task<ItemResponseDto> Show(int id)
        {
            var item = await context.Items
                .Include(o => o.Category)
                .Include(o => o.District)
                .Include(o => o.City)
                .Include(o => o.MyType)
                .Include(o => o.PropertyType)
                .Include(o => o.Status)
                .Include(o => o.Images)
                .FirstOrDefaultAsync(x => x.Id == id);


            var mapped = mapper.Map<ItemResponseDto>(item);

            return mapped;
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

        public async Task<ItemResponseDto?> UpdateAsync(int id, ItemUpdateRequestDto dto)
        {
            var item = await context
                                .Items.Include(i => i.Images)
                                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return null;

            mapper.Map(dto, item);

            if (dto.CategoryId.HasValue)
            {
                if (dto.CategoryId.Value <= 0)
                    throw new InvalidOperationException("رقم الفئة غير صالح.");

                var categoryExists = await context.Categories.AnyAsync(c => c.Id == dto.CategoryId.Value);
                if (!categoryExists)
                    throw new InvalidOperationException($"الفئة {dto.CategoryId.Value} غير موجودة.");

                item.CategoryId = dto.CategoryId.Value;
            }
            if (dto.CityId.HasValue)
            {
                if (dto.CityId.Value <= 0)
                    throw new InvalidOperationException("رقم المدينة غير صالح.");
                var cityExists = await context.Cities.AnyAsync(c => c.Id == dto.CityId.Value);
                if (!cityExists)
                    throw new InvalidOperationException($"المدينة {dto.CityId.Value} غير موجودة.");
                item.CityId = dto.CityId.Value;
            }
            if (dto.DistrictId.HasValue)
            {
                if (dto.DistrictId.Value <= 0)
                    throw new InvalidOperationException("رقم المنطقة غير صالح.");
                var districtExists = await context.Districts.AnyAsync(d => d.Id == dto.DistrictId.Value);
                if (!districtExists)
                    throw new InvalidOperationException($"المنطقة {dto.DistrictId.Value} غير موجودة.");
                item.DistrictId = dto.DistrictId.Value;
            }

            if (dto.PropertyTypeId.HasValue)
            {
                if (dto.PropertyTypeId.Value <= 0)
                    throw new InvalidOperationException("رقم نوع العقار غير صالح.");
                var propertyTypeExists = await context.PropertyTypes.AnyAsync(pt => pt.Id == dto.PropertyTypeId.Value);
                if (!propertyTypeExists)
                    throw new InvalidOperationException($"نوع العقار {dto.PropertyTypeId.Value} غير موجود.");
                item.PropertyTypeId = dto.PropertyTypeId.Value;
            }

            if (dto.StatusId.HasValue)
            {
                if (dto.StatusId.Value <= 0)
                    throw new InvalidOperationException("رقم الحالة غير صالح.");
                var statusExists = await context.Statuses.AnyAsync(s => s.Id == dto.StatusId.Value);
                if (!statusExists)
                    throw new InvalidOperationException($"الحالة {dto.StatusId.Value} غير موجودة.");
                item.StatusId = dto.StatusId.Value;
            }

            if (dto.AdvertiseNo.HasValue)
            {
                if (dto.AdvertiseNo.Value <= 0)
                    throw new InvalidOperationException("رقم الإعلان غير صالح.");
                item.AdvertiseNo = dto.AdvertiseNo.Value;
            }

            if (dto.AdNo.HasValue)
            {
                if (dto.AdNo.Value <= 0)
                    throw new InvalidOperationException("رقم الإعلان الفرعي غير صالح.");
                item.AdNo = dto.AdNo.Value;
            }


            if (dto.MyTypeId.HasValue)
            {
                if (dto.MyTypeId.Value <= 0)
                    throw new InvalidOperationException("رقم النوع غير صالح.");
                var typeExists = await context.Types.AnyAsync(t => t.Id == dto.MyTypeId.Value);
                if (!typeExists)
                    throw new InvalidOperationException($"النوع {dto.MyTypeId.Value} غير موجود.");
                item.MyTypeId = dto.MyTypeId.Value;
            }
            if (dto.Image is not null)
            {
                if (!string.IsNullOrEmpty(item.Image))
                {
                    var oldImagePath = Path.Combine(env.WebRootPath, item.Image.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }
                item.Image = await SaveImageAsync(dto.Image);
            }

            if (dto.Images is not null)
            {
                if (item.Images?.Any() == true)
        {
            foreach (var oldImage in item.Images)
            {
                var oldPath = Path.Combine(env.WebRootPath, oldImage.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }
        }
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

            return $"images/items/{fileName}";
        }
    }
}
