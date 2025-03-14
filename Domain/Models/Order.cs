using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingApp.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? ProductName { get; set; }
        public CustomerType CustomerType { get; set; }
        public string? DeliveryAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
