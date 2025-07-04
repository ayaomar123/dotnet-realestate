﻿using Microsoft.AspNetCore.Mvc;
using Realestate.DTOs;
using Realestate.DTOs.Item;
using Realestate.DTOs.Paginations;
using Realestate.Interfaces;

namespace Realestate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(IItemService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ItemFilterDto filter)
        {
            var result = await service.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<ItemResponseDto>>.Ok(result, "Fetched Successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var result = await service.Show(id);
            return Ok(ApiResponse<ItemResponseDto>.Ok(result, "Fetched Successfully"));
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ItemRequestDto dto)
        {
            var created = await service.CreateAsync(dto);
            var response = ApiResponse<ItemResponseDto>.Ok(created, "Created Successfully");

            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, response);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateBase(int id)
        {
            var item = await service.UpdateBaseAsync(id);
            return item == null ? BadRequest(ApiResponse<string>.Fail("Not found.")) : Ok(ApiResponse<ItemResponseDto>.Ok(item, "updated successfully."));
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ItemUpdateRequestDto dto)
        {
            var item = await service.UpdateAsync(id, dto);
            return item == null ? BadRequest(ApiResponse<string>.Fail("Not found.")) : Ok(ApiResponse<ItemResponseDto>.Ok(item, "updated successfully."));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await service.DeleteAsync(id);
            return success ? Ok(ApiResponse<ItemResponseDto>.Ok(null, "Deleted successfully.")) : BadRequest(ApiResponse<string>.Fail("Not found."));

        }
    }
}
