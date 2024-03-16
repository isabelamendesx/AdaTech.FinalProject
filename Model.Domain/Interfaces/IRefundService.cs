using Model.Domain.Common;
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
        Task<PaginatedResult<Refund>> GetAllByStatusPaginated(EStatus status, CancellationToken ct, int skip, int take);
        Task<Refund> ApproveRefund(uint Id, uint userId, CancellationToken ct);
        Task<Refund> RejectRefund(uint Id, uint userId, CancellationToken ct);
    }
}
