using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OrderProcessingApp.Application.Services;
using OrderProcessingApp.Domain;
using OrderProcessingApp.Domain.Interfaces.Repositories;
using OrderProcessingApp.Domain.Interfaces.Services;
using OrderProcessingApp.Infrastructure.Repositories;
using OrderProcessingApp.UI;
using Serilog;

var services = new ServiceCollection();

services.AddDbContext<OrderProcessingAppDbContext>(options =>
{
    options.UseInMemoryDatabase("OrderProcessingApp");
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/orderprocessingapp-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

services.AddLogging(configure =>
{
    configure.AddSerilog();
});
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<IOrderProcessingHistoryRepository, OrderProcessingHistoryRepository>();
services.AddScoped<ConsoleUI>();

var serviceProvider = services.BuildServiceProvider();

var consoleUI = serviceProvider.GetRequiredService<ConsoleUI>();
await consoleUI.Run();