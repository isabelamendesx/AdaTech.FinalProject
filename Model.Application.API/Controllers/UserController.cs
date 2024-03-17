using Identity.DTO;
using Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Serilog;
using System.Security.Claims;

namespace Model.Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private IIdentityService _identityService;
        private ILogger<UserController> _logger;

        public UserController(IIdentityService identityService, ILogger<UserController> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        [HttpPost("register")] 
        public async Task<ActionResult<UserRegisterResponse>> Register(UserRegisterRequest userRegister)
        {
            ValidateWithDataAnotation();

            var result = await _identityService.RegisterUser(userRegister);

            if (result.Success)
            {
                _logger.LogInformation("New User registered successfully");
                return Ok(result);
            }

            else if (result.Errors.Count > 0)
            {
                _logger.LogWarning("User registration failed with errors: {@Errors}", result.Errors);
                return BadRequest(result);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserRegisterResponse>> Login(UserLoginRequest userLogin)
        {
            ValidateWithDataAnotation();

            var result = await _identityService.Login(userLogin);

            if (result.Success)
            {
                _logger.LogInformation("User with id {@UserId} logged in successfully", HttpContext.Items["UserId"] as string);
                return Ok(result);
            }

            _logger.LogWarning("User login failed: {@UserRegisterResponse}", result);
            return Unauthorized(result);
        }

        [Authorize]
        [HttpPost("refresh-login")]
        public async Task<ActionResult<UserRegisterResponse>> RefreshLogin()
        {
            var userId = HttpContext.Items["UserId"] as string;

            if (userId == null)
            {
                _logger.LogWarning("User with ID not found in claims during refreshing login");
                throw new ResourceNotFoundException("User ID");
            }

            var result = await _identityService.LoginWithoutPassword(userId);
            if (result.Success)
            {
                _logger.LogInformation("User with ID {@UserId} login refreshed successfully", userId);
                return Ok(result);
            }

            _logger.LogWarning("Failed to refresh User with Id {@UserId} login: {@UserRegisterResponse}", result, userId);
            return Unauthorized();
        }
    }
}
