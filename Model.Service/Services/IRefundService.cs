﻿using Model.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Services
{
    public interface IRefundService
    {
        Task<Refund> CreateRefund(Refund refund);
        Task<IEnumerable<Refund?>> GetAllByStatus(EStatus status);
        Task<Refund> ApproveRefund(int Id);
        Task<Refund> RefuseRefund(int Id);
    }
}
