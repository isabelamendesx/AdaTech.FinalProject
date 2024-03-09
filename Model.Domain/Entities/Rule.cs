using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Entities
{
    public class Rule : BaseEntity
    {
        public Category CategoryID { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public bool Action { get; set; }
    }
}
