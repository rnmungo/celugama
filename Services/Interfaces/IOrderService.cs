using CeluGamaSystem.Dtos;
using CeluGamaSystem.Models;
using System;
using System.Collections.Generic;

namespace CeluGamaSystem.Services
{
    public interface IOrderService
    {
        Order GetOrder(long orderId);
        List<Order> SearchOrders(SearchOrder searchOrder);
        List<OrderTransportDto> GetOrdersToPrepare(SearchOrder filters);
    }
}
