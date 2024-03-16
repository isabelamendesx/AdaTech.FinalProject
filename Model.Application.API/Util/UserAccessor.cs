using Model.Domain.Interfaces;
using System.Security.Claims;

namespace Model.Application.API.Util
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            var userIdentity = _httpContextAccessor.HttpContext.User.Identity;

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                var claimsIdentity = userIdentity as ClaimsIdentity;

                return claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            return null;
        }
    }
}
