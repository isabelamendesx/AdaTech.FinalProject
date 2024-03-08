using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Exceptions
{
    public class RefundNotFoundException : Exception
    {
        public int StatusCode { get; set; }
        public RefundNotFoundException() : base("Refund could not be found.") {
            StatusCode = 404;
        }
    }
}
