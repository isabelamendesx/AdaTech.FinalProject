﻿using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services.DTO;
using Model.Service.Services.Util;
using Rule = Model.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Http.HttpResults;
using Model.Service.Services.Handlers;

namespace Model.Service.Services
{
    public class RefundService : IRefundService
    {
        private IRepository<Refund> _repository;
        private IRepository<RefundOperation> _operationRepository;
        private IRuleService _ruleService;
        private ICategoryService _categoryService;

        public RefundService(IRepository<Refund> repository, 
            IRepository<RefundOperation> operationRepository, IRuleService ruleService, ICategoryService categoryService)
        {
            _repository = repository;
            _operationRepository = operationRepository;
            _ruleService = ruleService;
            _categoryService = categoryService;
        }

        public async Task<Refund> CreateRefund(Refund refund, CancellationToken ct)
        {
            refund.Category = await _categoryService.GetById(refund.Category.Id, ct);

            if (refund.Category is null)
            {
                Log.Warning("Attempted to create a refund with an invalid category.");
                throw new ResourceNotFoundException("category");
            }

            var processResult = await ProcessRefund(refund.Category.Id, refund.Total, ct);

            refund.Status = processResult.Status;
            refund.CreateDate = DateTime.UtcNow;

            RefundOperation op = new RefundOperation()
            {
                UpdateDate = DateTime.UtcNow,
                ApprovedBy = 0
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

        public async Task<Refund> ApproveRefund(uint Id, uint userId, CancellationToken ct)
        {
            var refund = await _repository.GetById(Id, ct);

            if (refund is null)
            {
                Log.Warning("Attempted to approve a refund with an invalid ID.");
                throw new ResourceNotFoundException("Refund");
            }

            refund.Status = EStatus.Approved;
            RefundOperation op = new RefundOperation()
            {
                UpdateDate = DateTime.UtcNow,
                ApprovalRule = null,
                ApprovedBy = userId
            };
            refund.Operations.Add(op);

            await _repository.AddAsync(refund, ct);

            return refund;
        }

        public async Task<Refund> RejectRefund(uint Id, uint userId, CancellationToken ct)
        {
            var refund = await _repository.GetById(Id, ct);

            if (refund is null)
            {
                Log.Warning("Attempted to reject a refund with an invalid ID.");
                throw new ResourceNotFoundException("Refund");
            }

            refund.Status = EStatus.Rejected;

            RefundOperation op = new RefundOperation()
            {
                UpdateDate = DateTime.UtcNow,
                ApprovalRule = null,
                ApprovedBy = userId
            };
            refund.Operations.Add(op);

            await _repository.AddAsync(refund, ct);

            return refund;
        }

        private async Task<ProcessRefundResult> ProcessRefund(uint categoryId, decimal value, CancellationToken ct)
        {
            var rules = await _ruleService.GetRulesThatApplyToCategory(categoryId, ct);

            ApprovalMotorHandler handler = ApprovalMotorHandler.CreateChain(rules);

            return handler.Handle(value);
        }

    }
}
