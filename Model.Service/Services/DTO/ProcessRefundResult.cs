using Model.Domain.Entities;

namespace Model.Service.Services.DTO
{
    internal class ProcessRefundResult
    {
        public EStatus Status { get; set; }
        public Rule? Rule { get; set; }
    }
}
