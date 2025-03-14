using Microsoft.EntityFrameworkCore;
using OrderProcessingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingApp.Domain
{
    class OrderProcessingAppDbContext : DbContext
    {
        public OrderProcessingAppDbContext(DbContextOptions<OrderProcessingAppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Order> Orders { get; set; }

    }
}
