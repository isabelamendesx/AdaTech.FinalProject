using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Entities
{
    public class RefundOperation : BaseEntity
    {
        public Refund RefundID { get; set; }
        public DateTime UpdateDate { get; set; }
        public Rule ApprovalRule { get; set; }
        public int ApprovedBy { get; set; }

    }
}
