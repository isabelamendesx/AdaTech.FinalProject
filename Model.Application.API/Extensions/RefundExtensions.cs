using Model.Application.API.DTO.Response;
using Model.Domain.Entities;

namespace Model.Application.API.Extensions
{
    public static class RefundExtensions
    {
        public static RefundResponseDTO ToResponse(this Refund refund)
        {
            return new RefundResponseDTO
            {
                RefundId = refund.Id,
                Description = refund.Description,
                Category = refund.Category.Name,
                Value = refund.Total,
                Status = refund.Status.ToString(),
                OwnerId = refund.OwnerID,
            };
        }

        public static RefundResponseDTO ToDetailResponse(this Refund refund)
        {
            return new RefundResponseDTO
            {
                RefundId = refund.Id,
                Description = refund.Description,
                Category = refund.Category.Name,
                Value = refund.Total,
                Status = refund.Status.ToString(),
                OwnerId = refund.OwnerID,
                Operations = refund.Operations?.Select(op => op.ToResponse())
            };
        }
    }
}
