using Identity.DTO;
using Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IIdentityService _identityService;

        public UserController(IIdentityService identityService) 
            => _identityService = identityService;

        [HttpPost("register")] 
        public async Task<ActionResult<UserRegisterResponse>> Register(UserRegisterRequest userRegister)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _identityService.RegisterUser(userRegister);
            if (result.Success)
                return Ok(result);
            else if (result.Errors.Count > 0)
                return BadRequest(result);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserRegisterResponse>> Login(UserLoginRequest userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _identityService.Login(userLogin);
            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        [Authorize]
        [HttpPost("refresh-login")]
        public async Task<ActionResult<UserRegisterResponse>> RefreshLogin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var usuarioId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
                return BadRequest();

            var resultado = await _identityService.LoginWithoutPassword(usuarioId);
            if (resultado.Success)
                return Ok(resultado);

            return Unauthorized();
        }
    }
}
