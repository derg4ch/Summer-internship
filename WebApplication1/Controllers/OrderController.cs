﻿using System.Security.Claims;
using Logic;
using Logic.dto.order;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderInfoDto>>> GetAll()
        {
            var orders = await service.GetAllWithDetailsAsync();
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderInfoDto>> GetById(int id)
        {
            var order = await service.GetByIdWithDetailsAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Manager"))
            {
                Claim? claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null)
                {
                    return Unauthorized();
                }

                int userId = int.Parse(claim.Value);
                if (order.UserId != userId)
                {
                    return Forbid();
                }
            }

            return Ok(order);
        }

        [Authorize]
        [HttpGet("user/my")]
        public async Task<ActionResult<IEnumerable<OrderInfoDto>>> GetMyOrders()
        {
            Claim? claim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (claim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(claim.Value);
            
            var orders = await service.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderInfoDto>>> GetByStatus(string status)
        {
            var allOrders = await service.GetOrdersByStatusAsync(status);
            return Ok(allOrders);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderInfoDto>> Create(OrderNewDto newOrder)
        {
            Claim? claim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (claim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(claim.Value);
            newOrder.UserId = userId;

            var createdOrder = await service.CreateAsync(newOrder);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }

        [Authorize(Roles = "Manager")]
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

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await service.GetByIdWithDetailsAsync(id);
            
            if (order == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Manager"))
            {
                Claim? claim = User.FindFirst(ClaimTypes.NameIdentifier);
                
                if (claim == null)
                {
                    return Unauthorized();
                }

                int userId = int.Parse(claim.Value);
                if (order.UserId != userId)
                {
                    return Forbid();
                }
            }

            var result = await service.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("paginated")]
        public async Task<ActionResult<PagedList<OrderInfoDto>>> GetPagedOrders(int pageNumber = 1, int pageSize = 10)
        {
            var pagedOrders = await service.GetPagedOrdersAsync(pageNumber, pageSize);
            return Ok(pagedOrders);
        }
    }
}