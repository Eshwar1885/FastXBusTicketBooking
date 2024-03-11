using FastX.Models.DTOs;

namespace FastX.Interfaces
{
    public interface IAllUserService
    {
        public Task<LoginUserDTO> Login(LoginUserInputDTO user);
        public Task<LoginUserDTO> Register(RegisterUserDTO user);
    }
}



//using FastX.Models;

//namespace FastX.Interfaces
//{
//    public interface IAdminService
//    {
//        //public Task<Admin> Delete(string name);
//        //public Task<Admin> GetAsync(string name);
//        Task DeleteUserAsync(string username);


//    }
//}

