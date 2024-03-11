using Identity.DTO;

namespace Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<UserRegisterResponse> RegisterUser(UserRegisterRequest userRegister);

        Task<UserLoginResponse> Login(UserLoginRequest userLogin);
    }
}
