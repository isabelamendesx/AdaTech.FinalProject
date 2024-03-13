using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services.DTO;
using Model.Service.Services.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Services
{
    public class RefundService : IRefundService
    {
        private IRepository<Refund> _repository;
        private IRepository<RefundOperation> _operationRepository;
        private IRuleService _ruleService;

        public RefundService(IRepository<Refund> repository, 
            IRepository<RefundOperation> operationRepository, IRuleService ruleService)
        {
            _repository = repository;
            _operationRepository = operationRepository;
            _ruleService = ruleService;
        }

        public async Task<Refund> CreateRefund(Refund refund, CancellationToken ct)
        {
            var processResult = await ProcessRefund(refund.Category.Id, refund.Total, ct);
            refund.Status = processResult.Status;
            refund.CreateDate = DateTime.Now;

            RefundOperation op = new RefundOperation()
            {
                UpdateDate = DateTime.Now,
                ApprovalRule = processResult.Rule,
                ApprovedBy = 0,
                Refund = refund
            };

            refund.Operations.Add(op);

            await _repository.UpdateAsync(refund, ct);
            await _operationRepository.AddAsync(op, ct);

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

            if (refund == null)
                throw new RefundNotFoundException();

            refund.Status = EStatus.Approved;
            RefundOperation op = new RefundOperation()
            {
                UpdateDate = DateTime.Now,
                ApprovalRule = null,
                ApprovedBy = userId,
                Refund = refund

            };
            refund.Operations.Add(op);

            await _repository.UpdateAsync(refund, ct);
            await _operationRepository.AddAsync(op, ct);
            return refund;
        }

        public async Task<Refund> RefuseRefund(uint Id, uint userId, CancellationToken ct)
        {
            var refund = await _repository.GetById(Id, ct);

            if (refund == null)
                throw new RefundNotFoundException();

            refund.Status = EStatus.Rejected;

            RefundOperation op = new RefundOperation()
            {
                UpdateDate = DateTime.Now,
                ApprovalRule = null,
                ApprovedBy = userId,
                Refund = refund
            };
            refund.Operations.Add(op);

            await _repository.UpdateAsync(refund, ct);
            await _operationRepository.AddAsync(op, ct);

            return refund;
        }

        private async Task<ProcessRefundResult> ProcessRefund(uint categoryId, decimal value, CancellationToken ct)
        {
            var rulesThatReproveAny = await _ruleService.GetRulesToReproveAny(ct);

            Rule? reproveAnyResult = GetFirstMatchingRule(rulesThatReproveAny, value);

            if (reproveAnyResult is not null)
                return new ProcessRefundResult() { 
                    Status = EStatus.Rejected, 
                    Rule = reproveAnyResult
                };


            var rulesThatApproveAny = await _ruleService.GetRulesToApproveAny(ct);

            Rule? approveAnyResult = GetFirstMatchingRule(rulesThatApproveAny, value);

            if (approveAnyResult is not null)
                return new ProcessRefundResult()
                {
                    Status = EStatus.Approved,
                    Rule = approveAnyResult
                };



            var rulesThatApproveByCategory = await _ruleService
                .GetRulesToApproveByCategoryId(categoryId, ct);

            Rule? approveByCategoryResult = GetFirstMatchingRule(rulesThatApproveByCategory, value);

            if (approveByCategoryResult is not null)
                return new ProcessRefundResult()
                {
                    Status = EStatus.Approved,
                    Rule = approveByCategoryResult
                };

           

            return new ProcessRefundResult() { Status = EStatus.UnderEvaluation, Rule = null };
        }

        private Rule? GetFirstMatchingRule(IEnumerable<Rule?> rules, decimal value)
        {
            var rulesFuncs = RuleToFuncConverter.ConvertListOfRules(rules);

            var rulesFuncsResult = rulesFuncs
                .Select(rule => rule(value))
                .FirstOrDefault(result => result.FuncResult);

            if (rulesFuncsResult is not null)
                return rulesFuncsResult.Rule;
            

            return null;
        }   

    }
}
