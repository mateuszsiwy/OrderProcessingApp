using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using OrderProcessingApp.Application.Services;
using OrderProcessingApp.Domain;
using OrderProcessingApp.Domain.Interfaces.Repositories;
using OrderProcessingApp.Domain.Interfaces.Services;
using OrderProcessingApp.Infrastructure.Repositories;
using OrderProcessingApp.UI;

var services = new ServiceCollection();

services.AddDbContext<OrderProcessingAppDbContext>(options =>
{
    options.UseInMemoryDatabase("OrderProcessingApp");
});

services.AddLogging(configure => configure.AddConsole());
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<ConsoleUI>();

var serviceProvider = services.BuildServiceProvider();

var consoleUI = serviceProvider.GetRequiredService<ConsoleUI>();
await consoleUI.Run();