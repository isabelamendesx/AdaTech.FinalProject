using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Exceptions
{
    public class InternalErrorException : Exception
    {
        public int StatusCode { get; set; }
        public InternalErrorException(string occasion) : base($"An internal error occured during: {occasion}")
        {
            StatusCode = 400;
        }
    }
}
