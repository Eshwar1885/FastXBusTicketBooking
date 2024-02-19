using FastX.Models.DTOs;

namespace FastX.Interfaces
{
    public interface ITokenService
    {
        //public Task<string> GenerateToken(LoginUserInputDTO user);
        public Task<string> GenerateToken(string username, string role);

    }
}
