using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Entities
{
    public enum EStatus
    {
        Submitted = 1,
        Approved = 2,
        Rejected = 3,
        UnderEvaluation = 4,
        Paid = 5
    }
}
