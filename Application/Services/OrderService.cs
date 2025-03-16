using OrderProcessingApp.Domain.Exceptions;
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
        private readonly IOrderProcessingHistoryRepository _orderProcessingHistoryRepository;
        public OrderService(IOrderRepository orderRepository, IOrderProcessingHistoryRepository orderProcessingHistoryRepository)
        {
            _orderRepository = orderRepository;
            _orderProcessingHistoryRepository = orderProcessingHistoryRepository;
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
                throw new InvalidOrderStateException("Order must be in stock to be processed for shipping ");
            }

            if (order.OrderStatus == OrderStatus.Error || order.OrderStatus == OrderStatus.Closed)
            {
                throw new InvalidOrderStateException("Order is not available for shipping");
            }

            var previousStatus = order.OrderStatus;
            order.OrderStatus = OrderStatus.InDelivery;
            await _orderRepository.UpdateOrderAsync(order);

            await AddOrderHistoryAsync(order, previousStatus);
        }

        public async Task ProcessOrderToWarehouseAsync(int orderId)
        {

            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if(order.OrderStatus == OrderStatus.Error || order.OrderStatus == OrderStatus.Closed)
            {
                throw new InvalidOrderStateException("Order is not available for processing");
            }

            if (string.IsNullOrEmpty(order.DeliveryAddress))
            {
                order.OrderStatus = OrderStatus.Error;
                await _orderRepository.UpdateOrderAsync(order);
                throw new Exception("Delivery address is required");
            }
            var previousStatus = order.OrderStatus;
            if (order.PaymentMethod == PaymentMethod.Cash && order.Amount >= 2500)
            {
                order.OrderStatus = OrderStatus.ReturnedToClient;
            }
            else
            {
                order.OrderStatus = OrderStatus.InStock;
            }

            await _orderRepository.UpdateOrderAsync(order);

            await AddOrderHistoryAsync(order, previousStatus);
        }

        private async Task AddOrderHistoryAsync(Order order, OrderStatus previousStatus)
        {
            await _orderProcessingHistoryRepository.AddOrderHistoryAsync(new OrderProcessingHistory
            {
                OrderId = order.Id,
                PreviousStatus = previousStatus,
                NewStatus = order.OrderStatus,
                Timestamp = DateTime.Now,
                Information = "Order processed to warehouse"
            });
        }
    }
}
