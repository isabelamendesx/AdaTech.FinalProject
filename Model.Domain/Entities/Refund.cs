﻿
namespace Model.Domain.Entities
{
    public class Refund : BaseEntity
    {
        public decimal Total { get; set; }
        public string Description {  get; set; }
        public EStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string OwnerID { get; set; }
        public Category Category { get; set; }
        public List<RefundOperation> Operations { get; set; }

        public Refund()
        {
            Operations = new List<RefundOperation>();
        }

        public bool UpdateStatus(EStatus status)
        {
            this.Status = status;
            return true;
        }
    }
}
