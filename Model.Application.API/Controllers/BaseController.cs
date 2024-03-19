using Microsoft.AspNetCore.Mvc;
using Model.Service.Exceptions;
using System.Security.Claims;

namespace Model.Application.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected void ValidateWithDataAnotation()
        {
            if (!ModelState.IsValid)
                throw new ValidationException(ModelState);
        }

        protected string GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new UnauthorizedAccessException();

            return userId;  
        }

    }
}
