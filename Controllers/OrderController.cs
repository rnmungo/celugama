using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CeluGamaSystem.Dtos;
using CeluGamaSystem.Models;
using CeluGamaSystem.Services;
using CeluGamaSystem.Exceptions;

namespace CeluGamaSystem.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<SearchOrdersContainer>> GetOrders([FromQuery] SearchOrder searchOrder)
        {
            List<OrderTransportDto> orders = _service.GetOrdersToPrepare(searchOrder);
            SearchOrdersContainer container = new SearchOrdersContainer()
            {
                Results = orders,
                ShippingType = searchOrder.ShippingType
            };

            return Ok(container);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetOrder(long orderId)
        {
            Order order = _service.GetOrder(orderId);
            if (order == null)
            {
                throw new OrderFieldsException($"Order not found with ID {orderId}", 404);
            }

            return Ok(order.OrderItem);
        }
    }
}