﻿using Microsoft.Extensions.Logging;
using Model.Domain.Common;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services.DTO;
using Model.Service.Services.Handlers;

namespace Model.Service.Services
{
    public class RefundService : IRefundService
    {
        private IRepository<Refund> _repository;
        private IRuleService _ruleService;
        private ICategoryService _categoryService;
        private readonly ILogger<RefundService> _logger;

        public RefundService(IRepository<Refund> repository, IRuleService ruleService, 
                             ICategoryService categoryService, ILogger<RefundService> logger)
        {
            _repository = repository;
            _ruleService = ruleService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<Refund> CreateRefund(Refund refund, CancellationToken ct)
        {
            if (refund.Category.Id == 0)
            {
                _logger.LogWarning("Attempted to create a refund with an invalid category.");
                throw new ResourceNotFoundException("category");
            }

            refund.Category = await _categoryService.GetById(refund.Category.Id, ct);

            var processResult = await ProcessRefund(refund.Category.Id, refund.Total, ct);

            refund.Status = processResult.Status;
            refund.CreateDate = DateTime.UtcNow;

            var op = new RefundOperation()
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
            var refund = await GetById(Id, ct);

            ValidateIfRefundCanBeApprovedOrRejected(refund.Status);
            
            refund.Status = EStatus.Approved;

            AddRefundOperationToRefund(refund, userId);

            await _repository.UpdateAsync(refund, ct);

            return refund;      
        }
        public async Task<Refund> RejectRefund(uint Id, string userId, CancellationToken ct)
        {
            var refund = await GetById(Id, ct);

            ValidateIfRefundCanBeApprovedOrRejected(refund.Status);

            refund.Status = EStatus.Rejected;

            AddRefundOperationToRefund(refund, userId);

            await _repository.UpdateAsync(refund, ct);

            return refund;
        }

        public async Task<Refund> ChangeRefundStatus(uint Id, EStatus status, string userId, CancellationToken ct)
        {
            var refund = await GetById(Id, ct);
            
            refund.Status = status;

            AddRefundOperationToRefund(refund, userId);

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

        public async Task<Refund> GetById(uint id, CancellationToken ct)
        {
            var refund = await _repository.GetById(id, ct);

            if (refund is null)
            {
                _logger.LogInformation("Refund with ID {@CategoryId} not found", id);
                throw new ResourceNotFoundException("Refund");
            }

            return refund;
        }

        private void AddRefundOperationToRefund(Refund refund, string userId)
        {
            var op = new RefundOperation
            {
                UpdateDate = DateTime.UtcNow,
                ApprovalRule = null,
                ApprovedBy = userId
            };
            refund.Operations.Add(op);
        }

        private void ValidateIfRefundCanBeApprovedOrRejected(EStatus status)
        {
            if (status != EStatus.UnderEvaluation)
                throw new InvalidRefundException("The refund can only be approved or rejected if the status is UnderEvaluation.");
        }

    }
}
