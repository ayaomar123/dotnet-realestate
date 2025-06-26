
using Realestate.DTOs.Item;

namespace Realestate.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemResponseDto>> GetAllAsync();
        Task<ItemResponseDto> CreateAsync(ItemRequestDto dto);
        Task<ItemResponseDto?> UpdateAsync(int id, ItemRequestDto dto);
        Task<ItemResponseDto?> UpdateBaseAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
