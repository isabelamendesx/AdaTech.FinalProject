using Identity.DTO;

namespace Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<UserRegisterResponse> RegisterUser(UserRegisterRequest registerUser);

        Task<UserLoginResponse> Login(UserLoginRequest loginUser);
    }
}
