using Logic.dto.size;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/size")]
    public class SizeController : ControllerBase
    {
        private ISizeService service;

        public SizeController(ISizeService service)
        {
            this.service = service;
        }

        [HttpGet("with-count")]
        public async Task<ActionResult<IEnumerable<SizeInfoDto>>> GetAllWithClothingItemsCount()
        {
            var sizes = await service.GetAllWithClothingItemsCountAsync();
            return Ok(sizes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SizeInfoDto>> GetById(int id)
        {
            var size = await service.GetByIdAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            return Ok(size);
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<SizeInfoDto>> GetByName(string name)
        {
            var size = await service.GetByNameAsync(name);
            if (size == null)
            {
                return NotFound();
            }
            return Ok(size);
        }

        [HttpPost]
        public async Task<ActionResult<SizeInfoDto>> Create([FromBody] SizeNewDto newDto)
        {
            var created = await service.CreateAsync(newDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SizeInfoDto>> Update(int id, [FromBody] SizeEditDto editDto)
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
