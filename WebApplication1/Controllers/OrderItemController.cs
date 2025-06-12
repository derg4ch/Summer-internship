using System.Security.Claims;
using Logic.dto.order_item;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/orderItem")]
    [Authorize]
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
            try
            {
                var items = await service.GetAllWithDetailsAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemInfoDto>> GetByIdWithDetails(int id)
        {
            try
            {
                var item = await service.GetByIdWithDetailsAsync(id);
                if (item == null)
                {
                    return NotFound();
                }

                if (!User.IsInRole("Manager"))
                {
                    int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    if (item.OrderUserId != userId)
                    {
                        return Forbid();
                    }
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderItemInfoDto>>> GetByOrderId(int orderId)
        {
            try
            {
                if (!User.IsInRole("Manager"))
                {
                    int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    int orderUserId = await service.GetOrderUserIdAsync(orderId);

                    if (orderUserId != userId)
                    {
                        return Forbid();
                    }
                }

                var items = await service.GetByOrderIdAsync(orderId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemInfoDto>> Create([FromBody] OrderItemNewDto newDto)
        {
            try
            {
                if (!User.IsInRole("Manager"))
                {
                    int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    
                    if (newDto.UserId != userId)
                    {
                        return Forbid();
                    }

                    int orderUserId = await service.GetOrderUserIdAsync(newDto.OrderId);
                    if (orderUserId != userId)
                    {
                        return Forbid();
                    }
                }

                var created = await service.CreateAsync(newDto);
                return CreatedAtAction(nameof(GetByIdWithDetails), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderItemInfoDto>> Update(int id, [FromBody] OrderItemEditDto editDto)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}