using Microsoft.AspNetCore.Mvc;
using Realestate.DTOs.Category;
using Realestate.Interfaces;

namespace Realestate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController(ICityInterface service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await service.GetAllAsync());


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BaseRequestDto dto)
        {
            var created = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateBase(int id)
        {
            var updatedCategory = await service.UpdateBaseAsync(id);
            return updatedCategory == null ? NotFound() : Ok(updatedCategory);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BaseRequestDto dto)
        {
            var updatedCategory = await service.UpdateAsync(id, dto);
            return updatedCategory == null ? NotFound() : Ok(updatedCategory);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await service.DeleteAsync(id);
            return success ? Ok() : NotFound();
        }
    }
}
