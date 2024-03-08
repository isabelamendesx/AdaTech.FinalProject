using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
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

        public RefundService(IRepository<Refund> repository)
        {
            _repository = repository;
        }

       /* public Task<Refund> CreateRefund(Refund refund)
        {
            refund.Status = ProcessRefund(refund.Category, refund.Total);
            return _repository.AddAsync(refund);
        }*/


        public Task<IEnumerable<Refund?>> GetAllByStatus(EStatus status)
        {
            return _repository.GetByParameter(x => x.Status == status);
        }

        public Task<Refund> ApproveRefund(int Id)
        {
            var refund = _repository.GetById(Id).Result;

            if (refund == null)
                throw new RefundNotFoundException();

            refund.Status = EStatus.Approved;

            if (_repository.UpdateAsync(refund).Result)
                return Task.FromResult(refund);

            throw new InternalErrorException("refund update");
        }

        public Task<Refund> RefuseRefund(int Id)
        {
            var refund = _repository.GetById(Id).Result;

            if (refund == null)
                throw new RefundNotFoundException();

            refund.Status = EStatus.Rejected;

            if (_repository.UpdateAsync(refund).Result)
                return Task.FromResult(refund);

            throw new InternalErrorException("refund update");
        }

        private EStatus ProcessRefund(ECategory category, decimal value)
        {
            return category.CheckStatusByRules(value);
        }
    }
}
