using Model.Domain.Common;
using Model.Domain.Entities;

namespace Model.Domain.Interfaces
{
    public interface IRefundService
    {
        Task<Refund> CreateRefund(Refund refund, CancellationToken ct);
        Task<Refund> GetById(uint id, CancellationToken ct);
        Task<IEnumerable<Refund?>> GetAllByStatus(EStatus status, CancellationToken ct);
        Task<PaginatedResult<Refund>> GetAllByStatusPaginated(EStatus status, CancellationToken ct, int skip, int take);
        Task<Refund> ApproveRefund(uint Id, string userId, CancellationToken ct);
        Task<Refund> RejectRefund(uint Id, string userId, CancellationToken ct);
        Task<Refund> ChangeRefundStatus(uint Id, EStatus status, string userId, CancellationToken ct);
    }
}
