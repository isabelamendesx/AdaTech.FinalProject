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

       

        public Task<UserRegisterResponse> RegisterUser(UserRegisterRequest registerUser)
        {

            var identityUser = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = true

            };
            throw new NotImplementedException();
        }

        public Task<UserLoginResponse> Login(UserLoginRequest loginUser)
        {
            throw new NotImplementedException();
        }
    }
}
