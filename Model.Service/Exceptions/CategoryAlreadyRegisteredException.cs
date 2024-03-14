using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Exceptions
{
    public class CategoryAlreadyRegisteredException : Exception
    {
        public int StatusCode { get; set; }
        public CategoryAlreadyRegisteredException() : base("You can not create the same category twice.")
        {
            StatusCode = 400;
        }
    }
}
