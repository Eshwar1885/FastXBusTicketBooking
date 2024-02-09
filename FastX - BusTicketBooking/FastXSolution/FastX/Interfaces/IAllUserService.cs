using FastX.Models.DTOs;

namespace FastX.Interfaces
{
    public interface IAllUserService
    {
        public Task<LoginUserDTO> Login(LoginUserDTO user);
        public Task<LoginUserDTO> Register(RegisterUserDTO user);
    }
}
