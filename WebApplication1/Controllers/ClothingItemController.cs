using Logic;
using Logic.dto.clothing_item;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/clothingItem")]
    public class ClothingItemController : ControllerBase
    {
        private IClothingItemService service;

        public ClothingItemController(IClothingItemService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClothingItemInfoDto>>> GetAllWithDetails()
        {
            var items = await service.GetAllWithDetailsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClothingItemInfoDto>> GetByIdWithDetails(int id)
        {
            var item = await service.GetByIdWithDetailsAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ClothingItemInfoDto>> Create([FromBody] ClothingItemNewDto newDto)
        {
            var created = await service.CreateAsync(newDto);
            return CreatedAtAction(nameof(GetByIdWithDetails), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ClothingItemInfoDto>> Update(int id, [FromBody] ClothingItemEditDto editDto)
        {
            var updated = await service.UpdateAsync(id, editDto);
            if (updated == null)
            {
                return NotFound();
            }
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await service.DeleteAsync(id);
            
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedList<ClothingItemInfoDto>>> GetPagedClothingItems(int pageNumber = 1, int pageSize = 10)
        {
            var pagedItems = await service.GetPagedClothingItemsAsync(pageNumber, pageSize);
            return Ok(pagedItems);
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<PagedList<ClothingItemInfoDto>>> GetFilteredClothingItems([FromQuery] ClothingItemFilterDto filter)
        {
            var filteredItems = await service.GetFilteredClothingItemsAsync(filter);
            return Ok(filteredItems);
        }
    }
}
