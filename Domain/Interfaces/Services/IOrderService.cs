using OrderProcessingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingApp.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task ProcessOrderToWarehouseAsync(int orderId);
        Task ProcessOrderToShippingAsync(int orderId);
        Task AddOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersAsync();
    }
}
