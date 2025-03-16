using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderProcessingApp.Domain;
using OrderProcessingApp.Domain.Exceptions;
using OrderProcessingApp.Domain.Interfaces.Repositories;
using OrderProcessingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingApp.Infrastructure.Repositories
{
    public class OrderProcessingHistoryRepository : IOrderProcessingHistoryRepository
    {
        private readonly OrderProcessingAppDbContext _context;
        private readonly ILogger<OrderProcessingHistoryRepository> _logger;

        public OrderProcessingHistoryRepository(OrderProcessingAppDbContext context, ILogger<OrderProcessingHistoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddOrderHistoryAsync(OrderProcessingHistory orderProcessingHistory)
        {
            try
            {
                if (orderProcessingHistory == null)
                {
                    throw new ArgumentNullException("OrderProcessingHistory object is null");
                }
                _context.OrderProcessingHistories.Add(orderProcessingHistory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the order processing history");
                throw;
            }
        }

        public async Task<IEnumerable<OrderProcessingHistory>> GetOrderHistoriesAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    throw new OrderNotFoundException(orderId);
                }
                var orderProcessingHistories = await _context.OrderProcessingHistories.Where(x => x.OrderId == orderId).ToListAsync();
                if (orderProcessingHistories == null)
                {
                    throw new KeyNotFoundException("Order processing histories not found");
                }
                return orderProcessingHistories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the order processing histories");
                throw;
            }
        }

        public async Task<OrderProcessingHistory> GetOrderHistoryByIdAsync(int orderId)
        {
            try
            {
                var orderProcessingHistory = await _context.OrderProcessingHistories.FindAsync(orderId);
                if (orderProcessingHistory == null)
                {
                    throw new KeyNotFoundException($"Order processing history with id {orderId} not found");
                }
                return orderProcessingHistory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the order processing history by id");
                throw;
            }
        }
    }
}
