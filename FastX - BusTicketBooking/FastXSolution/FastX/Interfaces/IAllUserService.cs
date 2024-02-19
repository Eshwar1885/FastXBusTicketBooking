using FastX.Models.DTOs;

namespace FastX.Interfaces
{
    public interface IAllUserService
    {
        public Task<LoginUserDTO> Login(LoginUserInputDTO user);
        public Task<LoginUserDTO> Register(RegisterUserDTO user);
    }
}
