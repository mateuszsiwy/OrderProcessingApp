using OrderProcessingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingApp.Domain.Interfaces.Repositories
{
    public interface IOrderProcessingHistoryRepository
    {
        Task AddOrderHistoryAsync(OrderProcessingHistory orderProcessingHistory);
        Task<IEnumerable<OrderProcessingHistory>> GetOrderHistoriesAsync();
        Task<OrderProcessingHistory> GetOrderHistoryByIdAsync(int orderId);
    }
}
