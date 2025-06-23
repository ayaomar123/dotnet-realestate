using Realestate.DTOs.Status;

namespace Realestate.Interfaces
{
    public interface IStatusService
    {
        Task<List<StatusResponseDto>> GetAllAsync();
        Task<StatusResponseDto> CreateAsync(StatusRequestDto dto);
        Task<StatusResponseDto?> UpdateAsync(int id, StatusRequestDto dto);
        Task<StatusResponseDto?> UpdateStatusAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
