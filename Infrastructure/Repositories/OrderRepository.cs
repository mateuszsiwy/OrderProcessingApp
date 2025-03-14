using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderProcessingApp.Domain;
using OrderProcessingApp.Domain.Interfaces.Repositories;
using OrderProcessingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingApp.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderProcessingAppDbContext _context;
        private readonly ILogger<OrderRepository> _logger;
        public OrderRepository(OrderProcessingAppDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            try
            {
                if (order == null)
                {
                    _logger.LogWarning($"Cannot create order. Order object is null.");
                    throw new ArgumentNullException("Order object is null");
                }
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the order");
                throw;
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    _logger.LogWarning($"Cannot delete order. Order with id {id} not found.");
                    throw new KeyNotFoundException($"Order with id {id} not found.");
                }
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the order");
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    _logger.LogWarning($"Order with id {id} not found.");
                    throw new KeyNotFoundException($"Order with id {id} not found.");
                }
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the order");
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            try
            {
                return await _context.Orders.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the orders");
                throw;
            }
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            try
            {
                if (order == null)
                {
                    _logger.LogWarning($"Cannot update order. Order object is null.");
                    throw new ArgumentNullException("Order object is null");
                }
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the order");
                throw;
            }
        }
    }
}
