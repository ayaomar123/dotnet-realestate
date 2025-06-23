
using Realestate.DTOs.District;

namespace Realestate.Interfaces
{
    public interface IDistrictService
    {
        Task<List<DistrictResponseDto>> GetAllAsync();
        Task<DistrictResponseDto> CreateAsync(DistrictRequestDto dto);
        Task<DistrictResponseDto?> UpdateAsync(int id, DistrictRequestDto dto);
        Task<DistrictResponseDto?> UpdateStatusAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
