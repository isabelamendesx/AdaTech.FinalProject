using Microsoft.AspNetCore.Authorization;

namespace Identity.PolicyRequirements
{
    public class BusinessHourHandler : AuthorizationHandler<BusinessHoursRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BusinessHoursRequirement requirement)
        {
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);
            if (currentTime.Hour >= 8 && currentTime.Hour <= 18)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
