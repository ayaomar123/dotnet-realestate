
using Realestate.DTOs.Item;
using Realestate.DTOs.Paginations;

namespace Realestate.Interfaces
{
    public interface IItemService
    {
        Task<PaginatedResponse<ItemResponseDto>> GetAllAsync(ItemFilterDto filter);


        Task<ItemResponseDto> CreateAsync(ItemRequestDto dto);
        Task<ItemResponseDto?> UpdateAsync(int id, ItemRequestDto dto);
        Task<ItemResponseDto?> UpdateBaseAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
