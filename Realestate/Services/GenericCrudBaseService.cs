using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Realestate.Data;

namespace Realestate.Services
{
    public abstract class GenericCrudBaseService<TEntity, TRequestDto, TResponseDto>
    where TEntity : class, new()
    {
        protected readonly AppDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IWebHostEnvironment _env;
        protected readonly DbSet<TEntity> _dbSet;

        protected GenericCrudBaseService(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
            _dbSet = context.Set<TEntity>(); // هذا السطر يحل محل context.Categories
        }

        public virtual async Task<List<TResponseDto>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return _mapper.Map<List<TResponseDto>>(entities);
        }

        public virtual async Task<TResponseDto> CreateAsync(TRequestDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await HandleFileUploadAsync(dto, entity);
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TResponseDto>(entity);
        }

        public virtual async Task<TResponseDto?> UpdateAsync(int id, TRequestDto dto)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return default;

            _mapper.Map(dto, entity);
            await HandleFileUploadAsync(dto, entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TResponseDto>(entity);
        }

        public virtual async Task<TResponseDto?> UpdateStatusAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            var property = typeof(TEntity).GetProperty("IsActive");
            if (property == null || property.PropertyType != typeof(bool))
                throw new InvalidOperationException($"'{typeof(TEntity).Name}' does not contain a boolean property named 'IsActive'.");

            var currentValue = (bool)property.GetValue(entity)!;
            property.SetValue(entity, !currentValue);

            await _context.SaveChangesAsync();

            return _mapper.Map<TResponseDto>(entity);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        protected virtual Task HandleFileUploadAsync(TRequestDto dto, TEntity entity) => Task.CompletedTask;
    }

}
