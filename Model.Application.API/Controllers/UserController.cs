using Identity.DTO;
using Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Model.Service.Exceptions;
using Serilog;
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
            {
                Log.Warning("Invalid User model state: {@ModelState}", ModelState.Values);
                return UnprocessableEntity(ModelState);
            }

            var result = await _identityService.RegisterUser(userRegister);

            if (result.Success)
            {
                Log.Information("New User registered successfully");
                return Ok(result);
            }

            else if (result.Errors.Count > 0)
            {
                Log.Warning("User registration failed with errors: {@Errors}", result.Errors);
                return BadRequest(result);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserRegisterResponse>> Login(UserLoginRequest userLogin)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid User model state: {@ModelState}", ModelState.Values);
                return UnprocessableEntity(ModelState);
            }

            var result = await _identityService.Login(userLogin);

            if (result.Success)
            {
                Log.Information("User logged in successfully");
                return Ok(result);
            }

            Log.Warning("User login failed: {@UserRegisterResponse}", result);
            return Unauthorized(result);
        }

        [Authorize]
        [HttpPost("refresh-login")]
        public async Task<ActionResult<UserRegisterResponse>> RefreshLogin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                Log.Warning("User with ID not found in claims during refreshing login");
                throw new ResourceNotFoundException("User ID");
            }

            var result = await _identityService.LoginWithoutPassword(userId);
            if (result.Success)
            {
                Log.Information("User login refreshed successfully");
                return Ok(result);
            }

            Log.Warning("Failed to refresh user login: {@UserRegisterResponse}", result);
            return Unauthorized();
        }
    }
}
