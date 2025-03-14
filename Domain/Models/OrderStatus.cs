using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingApp.Domain.Models
{
    public enum OrderStatus
    {
        New,
        InStock,
        InDelivery,
        ReturnedToClient,
        Error,
        Closed
    }
}
