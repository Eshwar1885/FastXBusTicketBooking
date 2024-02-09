using FastX.Models.DTOs;
using FastX.Models;

namespace FastX.Mappers
{
    public class RegisterToAdmin
    {
        Admin admin;
        public RegisterToAdmin(RegisterUserDTO register)
        {
            admin = new Admin();
            admin.Name = register.Name;
            admin.ContactNumber = register.ContactNumber;
            admin.Username = register.Username;
            admin.Gender = register.Gender;
            admin.Address = register.Address;
        }
        public Admin GetAdmin()
        {
            return admin;
        }
    }
}
