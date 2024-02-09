using FastX.Models.DTOs;
using FastX.Models;

namespace FastX.Mappers
{
    public class RegisterToUser
    {
        User user;
        public RegisterToUser(RegisterUserDTO register)
        {
            user = new User();
            user.Name = register.Name;
            user.ContactNumber = register.ContactNumber;
            user.Username = register.Username;
            user.Gender = register.Gender;
            user.Address = register.Address;
        }
        public User GetUser()
        {
            return user;
        }
    }
}
