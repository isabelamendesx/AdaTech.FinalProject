using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.DTO
{
    internal class RefundRequestDto
    {
        public string Description { get; set; }
        public string Category { get; set;}
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
