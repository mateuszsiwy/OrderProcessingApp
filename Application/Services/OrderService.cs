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
    public class OrderService : IOrderService
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

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            
            if(order.OrderStatus != OrderStatus.InStock)
            {
                throw new InvalidOperationException("Order must be in stock to be processed for shipping ");
            }

            if (order.OrderStatus == OrderStatus.Error || order.OrderStatus == OrderStatus.Closed)
            {
                throw new InvalidOperationException("Order is not available for shipping");
            }
            order.OrderStatus = OrderStatus.InDelivery;
            await _orderRepository.UpdateOrderAsync(order);

        }

        public async Task ProcessOrderToWarehouseAsync(int orderId)
        {

            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if(order.OrderStatus == OrderStatus.Error || order.OrderStatus == OrderStatus.Closed)
            {
                throw new InvalidOperationException("Order is not available for processing");
            }

            if (string.IsNullOrEmpty(order.DeliveryAddress))
            {
                order.OrderStatus = OrderStatus.Error;
                await _orderRepository.UpdateOrderAsync(order);
                throw new Exception("Delivery address is required");
            }

            if (order.PaymentMethod == PaymentMethod.Cash && order.Amount >= 2500)
            {
                order.OrderStatus = OrderStatus.ReturnedToClient;
            }
            else
            {
                order.OrderStatus = OrderStatus.InStock;
            }

            await _orderRepository.UpdateOrderAsync(order);

        }
    }
}
