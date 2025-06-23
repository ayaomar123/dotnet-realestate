using Realestate.DTOs.Category;

namespace Realestate.Interfaces
{
    public interface IPropertyType
    {
        Task<List<BaseResponseDto>> GetAllAsync();
        Task<BaseResponseDto> CreateAsync(BaseRequestDto dto);
        Task<BaseResponseDto?> UpdateAsync(int id, BaseRequestDto dto);
        Task<BaseResponseDto?> UpdateBaseAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
