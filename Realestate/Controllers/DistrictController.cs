using Microsoft.AspNetCore.Mvc;
using Realestate.DTOs;
using Realestate.DTOs.District;
using Realestate.Interfaces;

namespace Realestate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController(IDistrictService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(ApiResponse<List<DistrictResponseDto>>.Ok(await service.GetAllAsync(), "Fetched Successfully"));


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DistrictRequestDto dto)
        {
            var created = await service.CreateAsync(dto);
            var response = ApiResponse<DistrictResponseDto>.Ok(created, "Created Successfully");
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, response);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var item = await service.UpdateStatusAsync(id);
            return item == null ? BadRequest(ApiResponse<string>.Fail("Not found.")) : Ok(ApiResponse<DistrictResponseDto>.Ok(item, "updated successfully."));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] DistrictRequestDto dto)
        {
            var item = await service.UpdateAsync(id, dto);
            return item == null ? BadRequest(ApiResponse<string>.Fail("Not found.")) : Ok(ApiResponse<DistrictResponseDto>.Ok(item, "updated successfully."));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await service.DeleteAsync(id);
            return success ? Ok(ApiResponse<DistrictResponseDto>.Ok(null, "Deleted successfully.")) : BadRequest(ApiResponse<string>.Fail("Not found."));
        }
    }
}
