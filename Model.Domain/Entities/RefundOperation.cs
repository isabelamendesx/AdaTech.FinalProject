using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Entities
{
    public class RefundOperation : BaseEntity
    {
        public DateTime UpdateDate { get; set; }
        public Rule ApprovalRule { get; set; }
        public uint ApprovedBy { get; set; }
        public Refund Refund { get; set; }

    }
}
