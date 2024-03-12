using Microsoft.AspNetCore.Identity;

using Identity.Interfaces;
using Identity.DTO;
using System.IdentityModel.Tokens.Jwt;
using Identity.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public IdentityService(SignInManager<IdentityUser> signInManager, 
                               UserManager<IdentityUser> userManager, 
                               IOptions<JwtOptions> jwtOptions)
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
            if (result.Succeeded)
                return await GenerateToken(userLogin.Email);

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

        private async Task<UserLoginResponse> GenerateToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var tokenClaims = await GetClaimsAndRoles(user);
            var expirationDate = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: tokenClaims,
                notBefore: DateTime.Now,
                expires: expirationDate,
                signingCredentials: _jwtOptions.SigningCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new UserLoginResponse
                (
                    success: true,
                    token: token,
                    expirationDate: expirationDate
                );
        }

        private async Task<IList<Claim>> GetClaimsAndRoles(IdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));

            foreach (var role in roles)
                claims.Add(new Claim("role", role));

            return claims;
        }


    }
}
