
namespace Model.Domain.Entities
{
    public class RefundOperation : BaseEntity
    {
        public DateTime UpdateDate { get; set; }
        public Rule? ApprovalRule { get; set; }
        public string ApprovedBy { get; set; }
    }
}
