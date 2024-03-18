using Microsoft.AspNetCore.Authorization;

namespace Identity.PolicyRequirements
{
    public class BusinessHoursRequirement : IAuthorizationRequirement
    {
        public BusinessHoursRequirement() { }
    }

}
