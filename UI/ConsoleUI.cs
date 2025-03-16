using OrderProcessingApp.Domain.Interfaces.Services;
using OrderProcessingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace OrderProcessingApp.UI
{
    class ConsoleUI
    {
        private readonly IOrderService _orderService;
        public ConsoleUI(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        public async Task Run()
        {
            AnsiConsole.Markup($"[green]Welcome to Order Processing App[/]\n");
            while (true)
            {
                try
                {
                    
                    var choice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("What do you want to do?")
                                .AddChoices(new[] {
                            "Add Sample Order", "Add Order", "Get Orders", "Process Order to Shipping", "Process Order to Warehouse", "Exit"
                                }));
                    switch (choice)
                    {
                        case "Add Sample Order":
                            await AddSampleOrder();
                            break;
                        case "Add Order":
                            await AddOrder();
                            break;
                        case "Get Orders":
                            await GetOrders();
                            break;
                        case "Process Order to Shipping":
                            await ProcessOrderToShipping();
                            break;
                        case "Process Order to Warehouse":
                            await ProcessOrderToWarehouse();
                            break;
                        case "Exit":
                            return;
                        default:
                            AnsiConsole.Markup("[red]Invalid choice[/]");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.Markup($"[red]An error occurred: {ex.Message}[/]\n");
                }
            }
        }

        private async Task AddSampleOrder()
        {
            var order = new Order
            {
                Amount = 1000,
                ProductName = "Sample Product",
                CustomerType = CustomerType.Individual,
                DeliveryAddress = "Sample Address",
                PaymentMethod = PaymentMethod.Cash,
                OrderStatus = OrderStatus.New,
            };
            await _orderService.AddOrderAsync(order);
        }

        private async Task AddOrder()
        {
            var amount = AnsiConsole.Ask<decimal>("Enter Amount:");
            var productName = AnsiConsole.Ask<string>("Enter Product Name:");
            var paymentMethod = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select Payment Method")
                    .AddChoices(new[] { "Cash", "Credit Card" }));
            var customerType = AnsiConsole.Prompt(
                new SelectionPrompt<CustomerType>()
                    .Title("Select Customer Type")
                    .AddChoices(new[] { CustomerType.Individual, CustomerType.Company }));
            var deliveryAddress = AnsiConsole.Ask<string>("Enter Delivery Address:");
            var order = new OrderProcessingApp.Domain.Models.Order
            {
                Amount = amount,
                ProductName = productName,
                CustomerType = customerType,
                DeliveryAddress = deliveryAddress,
                PaymentMethod = paymentMethod == "Cash" ? PaymentMethod.Cash : PaymentMethod.CreditCard,
                OrderStatus = OrderStatus.New,
            };
            await _orderService.AddOrderAsync(order);
            AnsiConsole.Markup("[green]Order added successfully[/]\n");
        }

        private async Task GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            if (orders.Any())
            {
                var table = new Table();
                table.AddColumn("Id");
                table.AddColumn("Amount");
                table.AddColumn("Product Name");
                table.AddColumn("Customer Type");
                table.AddColumn("Delivery Address");
                table.AddColumn("Payment Method");
                table.AddColumn("Order Status");
                foreach (var order in orders)
                {
                    table.AddRow(order.Id.ToString(), order.Amount.ToString(), order.ProductName ?? string.Empty, order.CustomerType.ToString(), order.DeliveryAddress ?? string.Empty, order.PaymentMethod.ToString(), order.OrderStatus.ToString());
                }
                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.Markup("[red]No orders found[/]\n");
            }
        }

        private async Task ProcessOrderToShipping()
        {
            var orderId = AnsiConsole.Ask<int>("Enter Order Id:");
            await _orderService.ProcessOrderToShippingAsync(orderId);
            AnsiConsole.Markup("[green]Order processed to shipping[/]\n");
        }

        private async Task ProcessOrderToWarehouse()
        {
            var orderId = AnsiConsole.Ask<int>("Enter Order Id:");
            await _orderService.ProcessOrderToWarehouseAsync(orderId);
            AnsiConsole.Markup("[green]Order processed to warehouse[/]\n");
        }
    }
}
