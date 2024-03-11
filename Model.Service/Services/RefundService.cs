using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services.DTO;
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

        public async Task<Refund> CreateRefund(Refund refund)
        {
            var processResult = ProcessRefund(refund.Category.Id, refund.Total);
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

            await _repository.UpdateAsync(refund);
            await _operationRepository.AddAsync(op);

            return await _repository.AddAsync(refund);
        }

        public async Task<IEnumerable<Refund?>> GetAll()
        {
            return await _repository.GetByParameter();
        }

        public async Task<IEnumerable<Refund?>> GetAllByStatus(EStatus status)
        {
            return await _repository.GetByParameter(x => x.Status == status);
        }

        public async Task<Refund> ApproveRefund(uint Id, uint userId)
        {
            var refund = await _repository.GetById(Id);

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

            await _repository.UpdateAsync(refund);
            await _operationRepository.AddAsync(op);
            return refund;
        }

        public async Task<Refund> RefuseRefund(uint Id, uint userId)
        {
            var refund = await _repository.GetById(Id);

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

            await _repository.UpdateAsync(refund);
            await _operationRepository.AddAsync(op);

            return refund;
        }

        private ProcessRefundResult ProcessRefund(uint categoryId, decimal value)
        {
            var reproveAny = GetReproveAny().Select(rule => rule(value)).FirstOrDefault(result => result.funcResult);

            if (reproveAny.rule != null)
                return new ProcessRefundResult() { Status = EStatus.Rejected, Rule = reproveAny.rule };


            var approveAny = GetApproveAny().Select(rule => rule(value)).FirstOrDefault(result => result.funcResult);

            if (approveAny.rule != null)
                return new ProcessRefundResult() { Status = EStatus.Approved, Rule = approveAny.rule };

            var approveByCategory = GetRulesToApproveByCategoryId(categoryId)
                .Select(rule => rule(value))
                .FirstOrDefault(result => result.funcResult);

            if (approveByCategory.rule != null)
                return new ProcessRefundResult() { Status = EStatus.Approved, Rule = approveAny.rule };

            return new ProcessRefundResult() { Status = EStatus.UnderEvaluation, Rule = null };
        }

        private List<Func<decimal, (bool funcResult, Rule rule)>> GetReproveAny()
        {
            var rules = _ruleService.GetRulesToReproveAny().Result;

            List<Func<decimal, (bool, Rule)>> reproveAny = new List<Func<decimal, (bool, Rule)>>();

            foreach (var rule in rules)
            {
                Func<decimal, (bool, Rule)> ruleToReprove;

                if (rule.MaxValue == null)
                {
                    ruleToReprove = (x) =>
                    {
                        return (x >= rule.MinValue, rule);
                    };
                }
                else
                {
                    ruleToReprove = (x) =>
                    {
                        return (x >= rule.MinValue && x <= rule.MaxValue, rule);
                    };
                }

                reproveAny.Add(ruleToReprove);
            }

            return reproveAny;
        }

        private List<Func<decimal, (bool funcResult, Rule rule)>> GetApproveAny()
        {
            var rules = _ruleService.GetRulesToApproveAny().Result;

            List<Func<decimal, (bool funcResult, Rule rule)>> reproveAny = new List<Func<decimal, (bool funcResult, Rule rule)>>();

            foreach (var rule in rules)
            {
                Func<decimal, (bool funcResult, Rule rule)> ruleToReprove;

                if (rule.MaxValue == null)
                {
                    ruleToReprove = (x) =>
                    {
                        return (x >= rule.MinValue, rule);
                    };
                }
                else
                {
                    ruleToReprove = (x) =>
                    {
                        return (x >= rule.MinValue && x <= rule.MaxValue, rule);
                    };
                }

                reproveAny.Add(ruleToReprove);
            }

            return reproveAny;
        }
        
        private List<Func<decimal, (bool funcResult, Rule rule)>> GetRulesToApproveByCategoryId(uint categoryId)
        {
            var rules = _ruleService.GetRulesToApproveByCategoryId(categoryId).Result;

            List<Func<decimal, (bool funcResult, Rule rule)>> reproveAny = new List<Func<decimal, (bool funcResult, Rule rule)>>();

            foreach (var rule in rules)
            {
                Func<decimal, (bool funcResult, Rule rule)> ruleToReprove;

                if (rule.MaxValue == null)
                {
                    ruleToReprove = (x) =>
                    {
                        return (x >= rule.MinValue, rule);
                    };
                }
                else
                {
                    ruleToReprove = (x) =>
                    {
                        return (x >= rule.MinValue && x <= rule.MaxValue, rule);
                    };
                }

                reproveAny.Add(ruleToReprove);
            }

            return reproveAny;
        }


    }
}
