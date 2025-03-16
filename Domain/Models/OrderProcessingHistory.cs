using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingApp.Domain.Models
{
    public class OrderProcessingHistory
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderStatus PreviousStatus { get; set; }
        public OrderStatus NewStatus { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Information { get; set; }
    }
}
