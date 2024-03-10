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
        private IPartialRepository<Refund> _repository;

        public RefundService(IPartialRepository<Refund> repository)
        {
            _repository = repository;
        }

        public async Task<Refund> CreateRefund(Refund refund)
        {
            refund.Status = ProcessRefund(refund.Category, refund.Total);
            refund.CreateDate = DateTime.Now;
            refund.LastUpdate = DateTime.Now;
            return await _repository.AddAsync(refund);
        }


        public async Task<IEnumerable<Refund?>> GetAllByStatus(EStatus status)
        {
            return await _repository.GetByParameter(x => x.Status == status);
        }

        public async Task<Refund> ApproveRefund(int Id)
        {
            var refund = await _repository.GetById(Id);

            if (refund == null)
                throw new RefundNotFoundException();

            refund.Status = EStatus.Approved;
            refund.LastUpdate = DateTime.Now;

            await _repository.UpdateAsync(refund);
            return refund;
        }

        public async Task<Refund> RefuseRefund(int Id)
        {
            var refund = await _repository.GetById(Id);

            if (refund == null)
                throw new RefundNotFoundException();

            refund.Status = EStatus.Rejected;
            refund.LastUpdate = DateTime.Now;

            await _repository.UpdateAsync(refund);
            return refund;
        }

        private EStatus ProcessRefund(ECategory category, decimal value)
        {
            return category.CheckStatusByRules(value);
        }
    }
}
