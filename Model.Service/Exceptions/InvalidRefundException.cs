using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Exceptions
{
    public class InvalidRefundException : Exception
    {
        public int StatusCode { get; set; }

        public InvalidRefundException(string message) : base(message)
        {
            StatusCode = 400;
        }
    }
}
