using Microsoft.AspNetCore.Mvc;
using Realestate.DTOs;
using Realestate.DTOs.Status;
using Realestate.Interfaces;

namespace Realestate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController(IStatusService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(ApiResponse<List<StatusResponseDto>>.Ok(await service.GetAllAsync(), "Fetched Successfully"));

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StatusRequestDto dto)
        {
            var created = await service.CreateAsync(dto);
            var response = ApiResponse<StatusResponseDto>.Ok(created, "Created Successfully");
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, response);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateBase(int id)
        {
            var item = await service.UpdateStatusAsync(id);
            return item == null ? BadRequest(ApiResponse<string>.Fail("Not found.")) : Ok(ApiResponse<StatusResponseDto>.Ok(item, "updated successfully."));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] StatusRequestDto dto)
        {
            var item = await service.UpdateAsync(id, dto);
            return item == null ? BadRequest(ApiResponse<string>.Fail("Not found.")) : Ok(ApiResponse<StatusResponseDto>.Ok(item, "updated successfully."));

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await service.DeleteAsync(id);
            return success ? Ok(ApiResponse<StatusResponseDto>.Ok(null, "Deleted successfully.")) : BadRequest(ApiResponse<string>.Fail("Not found."));
        }
    }
}
