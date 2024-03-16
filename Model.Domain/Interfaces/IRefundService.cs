using Model.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Interfaces
{
    public interface IRefundService
    {
        Task<Refund> CreateRefund(Refund refund, CancellationToken ct);
        Task<IEnumerable<Refund?>> GetAllByStatus(EStatus status, CancellationToken ct);
        Task<Refund> ApproveRefund(uint Id, string userId, CancellationToken ct);
        Task<Refund> RejectRefund(uint Id, string userId, CancellationToken ct);
    }
}
