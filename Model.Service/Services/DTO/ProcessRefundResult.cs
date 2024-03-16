using Model.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Services.DTO
{
    internal class ProcessRefundResult
    {
        public EStatus Status { get; set; }
        public Rule? Rule { get; set; }
    }
}
