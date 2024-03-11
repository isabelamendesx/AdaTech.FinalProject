using Model.Application.API.DTO;

namespace Model.Application.API.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<UserRegisterResponse> RegisterUser(UserRegisterRequest registerUser);

        Task<UserLoginResponse> Login(UserLoginRequest loginUser);
    }
}
