using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services.DTO;
using Microsoft.Extensions.Logging;
using Model.Service.Services.Handlers;
using Model.Domain.Common;

namespace Model.Service.Services
{
    public class RefundService : IRefundService
    {
        private IRepository<Refund> _repository;
        private IRuleService _ruleService;
        private ICategoryService _categoryService;
        private readonly ILogger<RefundService> _logger;

        public RefundService(IRepository<Refund> repository, 
            IRepository<RefundOperation> operationRepository, IRuleService ruleService, 
            ICategoryService categoryService, ILogger<RefundService> logger)
        {
            _repository = repository;
            _ruleService = ruleService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<Refund> CreateRefund(Refund refund, CancellationToken ct)
        {
            refund.Category = await _categoryService.GetById(refund.Category.Id, ct);

            if (refund.Category is null)
            {
                _logger.LogWarning("Attempted to create a refund with an invalid category.");
                throw new ResourceNotFoundException("category");
            }

            var processResult = await ProcessRefund(refund.Category.Id, refund.Total, ct);

            refund.Status = processResult.Status;
            refund.CreateDate = DateTime.UtcNow;

            RefundOperation op = new RefundOperation()
            {
                UpdateDate = DateTime.UtcNow,
                ApprovedBy = "0"
            };

            if (processResult.Rule is not null)
                op.ApprovalRule = await _ruleService.GetById(processResult.Rule.Id, ct);

            refund.Operations.Add(op);

            return await _repository.AddAsync(refund, ct);
        }

        public async Task<IEnumerable<Refund?>> GetAll(CancellationToken ct)
        {
            return await _repository.GetByParameter(ct);
        }

        public async Task<IEnumerable<Refund?>> GetAllByStatus(EStatus status, CancellationToken ct)
        {
            return await _repository.GetByParameter(ct, (x => x.Status == status));
        }

        public async Task<Refund> ApproveRefund(uint Id, string userId, CancellationToken ct)
        {
            var refund = await _repository.GetById(Id, ct);

            if(refund.Status != EStatus.UnderEvaluation)
                throw new InvalidRefundException("The refund can only be approved if the status is UnderEvaluation.");

            refund.Status = EStatus.Approved;
            RefundOperation op = new RefundOperation()
            {
                UpdateDate = DateTime.UtcNow,
                ApprovalRule = null,
                ApprovedBy = userId
            };

            refund.Operations.Add(op);

            await _repository.UpdateAsync(refund, ct);

            return refund;      
        }

        public async Task<Refund> RejectRefund(uint Id, string userId, CancellationToken ct)
        {
            var refund = await _repository.GetById(Id, ct);

            if (refund.Status != EStatus.UnderEvaluation)
                throw new InvalidRefundException("The refund can only be rejected if the status is UnderEvaluation.");

            refund.Status = EStatus.Rejected;

            RefundOperation op = new RefundOperation()
            {
                UpdateDate = DateTime.UtcNow,
                ApprovalRule = null,
                ApprovedBy = userId
            };
            refund.Operations.Add(op);

            await _repository.UpdateAsync(refund, ct);

            return refund;
        }

        public async Task<Refund> ChangeRefundStatus(uint Id, EStatus status, string userId, CancellationToken ct)
        {
            var refund = await _repository.GetById(Id, ct);

            refund.Status = status;

            RefundOperation op = new RefundOperation()
            {
                UpdateDate = DateTime.UtcNow,
                ApprovalRule = null,
                ApprovedBy = userId
            };

            refund.Operations.Add(op);

            await _repository.UpdateAsync(refund, ct);

            return refund;
        }

        private async Task<ProcessRefundResult> ProcessRefund(uint categoryId, decimal value, CancellationToken ct)
        {
            var rules = await _ruleService.GetRulesThatApplyToCategory(categoryId, ct);

            ApprovalMotorHandler handler = ApprovalMotorHandler.CreateChain(rules);

            return handler.Handle(value);
        }

        public async Task<PaginatedResult<Refund>> GetAllByStatusPaginated(EStatus status, CancellationToken ct, int skip, int take)
        {
            return await _repository.GetPaginatedByParameter(ct, skip, take);
        }

        public async Task<Refund?> GetById(uint id, CancellationToken ct)
        {
            var refund = await _repository.GetById(id, ct);

            if (refund is null)
            {
                _logger.LogInformation("Refund with ID {@CategoryId} not found", id);
                throw new ResourceNotFoundException("Refund");
            }

            return refund;
        }
    }
}
