using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingApp.Domain.Exceptions
{
    public class InvalidOrderStateException : Exception
    {
        public InvalidOrderStateException(string message) : base(message)
        {
        }
    }
}
