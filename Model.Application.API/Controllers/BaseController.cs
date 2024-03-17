using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Model.Service.Exceptions;

namespace Model.Application.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected void ValidateWithDataAnotation()
        {
            if (!ModelState.IsValid)
                throw new ValidationException(ModelState);
        }

    }
}
