using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessingApp.Domain;
using OrderProcessingApp.Domain.Interfaces.Repositories;
using OrderProcessingApp.Infrastructure.Repositories;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddDbContext<OrderProcessingAppDbContext>(options =>
{
    options.UseInMemoryDatabase("OrderProcessingApp");
});

services.AddScoped<IOrderRepository, OrderRepository>();
