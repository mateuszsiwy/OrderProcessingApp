using OrderProcessingApp.Domain.Interfaces.Repositories;
using OrderProcessingApp.Domain.Interfaces.Services;
using OrderProcessingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingApp.Application.Services
{
    class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task AddOrderAsync(Order order)
        {
            await _orderRepository.CreateOrderAsync(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetOrdersAsync();
        }

        public async Task ProcessOrderToShippingAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                
                await Task.Delay(5000);

                order.OrderStatus = OrderStatus.InDelivery;
                await _orderRepository.UpdateOrderAsync(order);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task ProcessOrderToWarehouseAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order.PaymentMethod == PaymentMethod.Cash && order.Amount >= 2500)
                {
                    order.OrderStatus = OrderStatus.ReturnedToClient;
                }
                else if (string.IsNullOrEmpty(order.DeliveryAddress))
                {
                    order.OrderStatus = OrderStatus.Error;
                    throw new Exception("Delivery address is required");
                }

                order.OrderStatus = OrderStatus.InStock;
                await _orderRepository.UpdateOrderAsync(order);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
