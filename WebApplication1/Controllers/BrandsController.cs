using Logic.dto.brand;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService brandService;

        public BrandsController(IBrandService brandService)
        {
            this.brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandInfoDto>>> GetAll()
        {
            var brands = await brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BrandInfoDto>> GetById(int id)
        {
            var brand = await brandService.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound(new { Message = $"Brand with Id={id} not found." });
            }

            return Ok(brand);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult<BrandInfoDto>> Create([FromBody] BrandNewDto newBrand)
        {
            if (newBrand == null)
            {
                return BadRequest(new { Message = "Brand data is null." });
            }

            var createdBrand = await brandService.CreateAsync(newBrand);
            return CreatedAtAction(nameof(GetById), new { id = createdBrand.Id }, createdBrand);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BrandEditDto updatedBrand)
        {
            if (updatedBrand == null)
            {
                return BadRequest(new { Message = "Updated brand data is null." });
            }

            var updateResult = await brandService.UpdateAsync(id, updatedBrand);
            if (!updateResult)
            {
                return NotFound(new { Message = $"Brand with Id={id} not found." });
            }

            return NoContent();
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await brandService.DeleteAsync(id);
            if (!deleteResult)
            {
                return NotFound(new { Message = $"Brand with Id={id} not found." });
            }

            return NoContent();
        }
    }
}
