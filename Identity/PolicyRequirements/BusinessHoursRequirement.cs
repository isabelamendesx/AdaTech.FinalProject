using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.PolicyRequirements
{
    public class BusinessHoursRequirement : IAuthorizationRequirement
    {
        public BusinessHoursRequirement() { }
    }

}
