using Logic.dto.order;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private IOrderService service;

        public OrderController(IOrderService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderInfoDto>>> GetAll()
        {
            var orders = await service.GetAllWithDetailsAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderInfoDto>> GetById(int id)
        {
            var order = await service.GetByIdWithDetailsAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderInfoDto>>> GetByUserId(int userId)
        {
            var orders = await service.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderInfoDto>>> GetByStatus(string status)
        {
            var orders = await service.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderInfoDto>> Create(OrderNewDto newOrder)
        {
            var createdOrder = await service.CreateAsync(newOrder);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderInfoDto>> UpdateStatus(int id, OrderEditDto editDto)
        {
            var updatedOrder = await service.UpdateStatusAsync(id, editDto);
            if (updatedOrder == null)
            {
                return NotFound();
            }
            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await service.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
