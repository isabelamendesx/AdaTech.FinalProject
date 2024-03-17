using Model.Application.API.DTO.Response;
using Model.Domain.Entities;

namespace Model.Application.API.Extensions
{
    public static class OperationExtensions
    {
        public static RefundOperationResponseDTO ToResponse(this RefundOperation operation)
        {
            string approvedBy = operation.ApprovedBy.Equals("0") ? "System" : operation.ApprovedBy;

            return new RefundOperationResponseDTO()
            {
                Date = operation.UpdateDate.ToString("dd/MM/yyyy HH:mm:ss"),
                Rule = operation.ApprovalRule?.ToResponse(),
                ApprovedBy = approvedBy
            };
        }
    }
}
