using System.Security.Claims;
using Logic.dto.order_item;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/orderItem")]
    [Authorize] // Авторизація для всіх методів, можна адаптувати
    public class OrderItemController : ControllerBase
    {
        private IOrderItemService service;

        public OrderItemController(IOrderItemService service)
        {
            this.service = service;
        }

        [Authorize(Roles = "Manager")]
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

            if (!User.IsInRole("Manager"))
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (item.OrderUserId != userId) // Припустимо, у OrderItemInfoDto є поле OrderUserId
                {
                    return Forbid();
                }
            }

            return Ok(item);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderItemInfoDto>>> GetByOrderId(int orderId)
        {
            if (!User.IsInRole("Manager"))
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var orderUserId = await service.GetOrderUserIdAsync(orderId); // метод, який повертає UserId власника замовлення

                if (orderUserId != userId)
                {
                    return Forbid();
                }
            }

            var items = await service.GetByOrderIdAsync(orderId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemInfoDto>> Create([FromBody] OrderItemNewDto newDto)
        {
            if (!User.IsInRole("Manager"))
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (newDto.UserId != userId) // Припустимо, newDto містить UserId або треба перевірити належність замовлення
                {
                    return Forbid();
                }
            }

            var created = await service.CreateAsync(newDto);
            return CreatedAtAction(nameof(GetByIdWithDetails), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderItemInfoDto>> Update(int id, [FromBody] OrderItemEditDto editDto)
        {
            var existingItem = await service.GetByIdWithDetailsAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Manager"))
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (existingItem.OrderUserId != userId)
                {
                    return Forbid();
                }
            }

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
            var existingItem = await service.GetByIdWithDetailsAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Manager"))
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (existingItem.OrderUserId != userId)
                {
                    return Forbid();
                }
            }

            var deleted = await service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
