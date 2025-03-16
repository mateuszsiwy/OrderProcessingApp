using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderProcessingApp.Application.Services;
using OrderProcessingApp.Domain.Interfaces.Repositories;
using OrderProcessingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderProcessingApp
{
    [TestClass]
    public sealed class OrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private OrderService _orderService;
        private Mock<IOrderProcessingHistoryRepository> _orderProcessingHistoryRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderProcessingHistoryRepositoryMock = new Mock<IOrderProcessingHistoryRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object, _orderProcessingHistoryRepositoryMock.Object);
        }

        [TestMethod]
        public async Task AddOrderAsync_Should_Call_CreateOrderAsync()
        {
            var order = new Order { Id = 1, Amount = 1000 };

            await _orderService.AddOrderAsync(order);

            _orderRepositoryMock.Verify(r => r.CreateOrderAsync(order), Times.Once);
        }

        [TestMethod]
        public async Task ProcessOrderToWarehouseAsync_Should_ReturnToClient_When_CashAndAmountAbove2500()
        {
            var order = new Order
            {
                Id = 1,
                Amount = 3000,
                PaymentMethod = PaymentMethod.Cash,
                DeliveryAddress = "Some Street",
                OrderStatus = OrderStatus.New
            };

            _orderRepositoryMock.Setup(r => r.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);

            await _orderService.ProcessOrderToWarehouseAsync(order.Id);

            Assert.AreEqual(OrderStatus.ReturnedToClient, order.OrderStatus);
        }
        [TestMethod]
        public async Task ProcessOrderToWarehouseAsync_Should_MoveToWarehouse_When_AmountLessThan2500()
        {
            var order = new Order
            {
                Id = 1,
                Amount = 2000,
                PaymentMethod = PaymentMethod.Cash,
                DeliveryAddress = "Some Street",
                OrderStatus = OrderStatus.New
            };
        
            _orderRepositoryMock.Setup(r => r.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);
        
            await _orderService.ProcessOrderToWarehouseAsync(order.Id);
        
            Assert.AreEqual(OrderStatus.InStock, order.OrderStatus);
        }
        [TestMethod]
        public async Task ProcessOrderToWarehouseAsync_Should_MoveToWarehouse_When_AmountMoreThan2500_And_PayedByCreditCard()
        {
            var order = new Order
            {
                Id = 1,
                Amount = 4000,
                PaymentMethod = PaymentMethod.CreditCard,
                DeliveryAddress = "Some Street",
                OrderStatus = OrderStatus.New
            };

            _orderRepositoryMock.Setup(r => r.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);

            await _orderService.ProcessOrderToWarehouseAsync(order.Id);

            Assert.AreEqual(OrderStatus.InStock, order.OrderStatus);
        }
        [TestMethod]
        public async Task ProcessOrderToWarehouseAsync_Should_SetError_When_NoDeliveryAddress()
        {
            var order = new Order
            {
                Id = 2,
                Amount = 1000,
                PaymentMethod = PaymentMethod.CreditCard,
                DeliveryAddress = string.Empty,
                OrderStatus = OrderStatus.New
            };

            _orderRepositoryMock.Setup(r => r.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);

            await Assert.ThrowsExceptionAsync<Exception>(async () =>
                await _orderService.ProcessOrderToWarehouseAsync(order.Id)
            );

            Assert.AreEqual(OrderStatus.Error, order.OrderStatus);
        }

        [TestMethod]
        public async Task ProcessOrderToShippingAsync_Should_ChangeStatusToInDelivery()
        {
            var order = new Order
            {
                Id = 3,
                Amount = 2000,
                PaymentMethod = PaymentMethod.CreditCard,
                DeliveryAddress = "Delivery Address",
                OrderStatus = OrderStatus.InStock
            };

            _orderRepositoryMock.Setup(r => r.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);

            await _orderService.ProcessOrderToShippingAsync(order.Id);

            Assert.AreEqual(OrderStatus.InDelivery, order.OrderStatus);
        }

        [TestMethod]
        public async Task ProcessOrderToShippingAsync_Should_ThrowException_When_OrderNotFound()
        {
            _orderRepositoryMock.
                Setup(r => r.GetOrderByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new KeyNotFoundException("Order with id 111 not found"));
                

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
                await _orderService.ProcessOrderToShippingAsync(111)
            );
        }
    }
}