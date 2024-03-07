using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Entities
{
    public class Refund : BaseEntity
    {
        public decimal Total { get; set; }
        public ECategory Category { get; set; }
        public string Description {  get; set; }
        public EStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdate { get; set; }

        public bool UpdateStatus(EStatus status)
        {
            if(this.Status == EStatus.Approved || this.Status == EStatus.Rejected)
                return false;


            this.Status = status;
            return true;
        }
    }
}
