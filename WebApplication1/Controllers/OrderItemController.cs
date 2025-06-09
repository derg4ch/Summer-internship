using Logic.dto.order_item;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/orderItem")]
    public class OrderItemController : ControllerBase
    {
        private IOrderItemService service;

        public OrderItemController(IOrderItemService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemInfoDto>>> GetAllWithDetails()
        {
            var items = await service.GetAllWithDetailsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemInfoDto>> GetByIdWithDetails(int id)
        {
            var item = await service.GetByIdWithDetailsAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderItemInfoDto>>> GetByOrderId(int orderId)
        {
            var items = await service.GetByOrderIdAsync(orderId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemInfoDto>> Create([FromBody] OrderItemNewDto newDto)
        {
            var created = await service.CreateAsync(newDto);
            return CreatedAtAction(nameof(GetByIdWithDetails), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderItemInfoDto>> Update(int id, [FromBody] OrderItemEditDto editDto)
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
