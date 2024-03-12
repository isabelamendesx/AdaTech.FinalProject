using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Identity.Interfaces;
using Identity.DTO;

namespace Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public IdentityService(SignInManager<IdentityUser> signInManager, 
                               UserManager<IdentityUser> userManager, 
                               JwtOptions jwtOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<UserRegisterResponse> RegisterUser(UserRegisterRequest userRegister)
        {

            var identityUser = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = true

            };
            var result = await _userManager.CreateAsync(identityUser, userRegister.Password);
            if (result.Succeeded)
                await _userManager.SetLockoutEnabledAsync(identityUser, false);

            var userRegisterResponse = new UserRegisterResponse(result.Succeeded);
            if (!result.Succeeded && result.Errors.Count() > 0)
                userRegisterResponse.AddErrors(result.Errors.Select(r => r.Description));


            return userRegisterResponse;
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest userLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);
                //if (result.Succeeded)
                //   return await GenerateToken(userLogin.Email);

            var userLoginResponse = new UserLoginResponse(result.Succeeded);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    userLoginResponse.AddError("This account is blocked");
                else if (result.IsNotAllowed)
                    userLoginResponse.AddError("This account does not have permission to log in");
                else if (result.RequiresTwoFactor)
                    userLoginResponse.AddError("This account does not have permission to log in");
                else userLoginResponse.AddError("Username or password is incorrect");

            }

            return userLoginResponse;
        }
    }
}
