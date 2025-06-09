using Logic.dto.clothing_item;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/clothingItem")]
    public class ClothingItemController : ControllerBase
    {
        private readonly IClothingItemService service;

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
        public async Task<ActionResult<ClothingItemInfoDto>> Create([FromBody] ClothingItemNewDto newDto)
        {
            var created = await service.CreateAsync(newDto);
            return CreatedAtAction(nameof(GetByIdWithDetails), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
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
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await service.DeleteAsync(id);
            
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
