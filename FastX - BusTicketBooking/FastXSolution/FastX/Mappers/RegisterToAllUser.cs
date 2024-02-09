using FastX.Models.DTOs;
using FastX.Models;
using System.Security.Cryptography;
using System.Text;

namespace FastX.Mappers
{
    public class RegisterToAllUser
    {
        AllUser alluser;
        public RegisterToAllUser(RegisterUserDTO register)
        {
            alluser = new AllUser();
            alluser.Username = register.Username;
            alluser.Role = register.Role;
            GetPassword(register.Password);
        }
        void GetPassword(string password)
        {
            HMACSHA512 hmac = new HMACSHA512();
            alluser.Key = hmac.Key;
            alluser.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public AllUser GetAllUser()
        {
            return alluser;
        }
    }
}
