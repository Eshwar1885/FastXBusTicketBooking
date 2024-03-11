using FastX.Models;
using FastX.Models.DTOs;

namespace FastX.Mappers
{
    public class RegisterToBusOperator
    {
        BusOperator busOperator;
        public RegisterToBusOperator(RegisterUserDTO register)
        {
            busOperator = new BusOperator();
            busOperator.Name = register.Name;
            busOperator.ContactNumber = register.ContactNumber;
            busOperator.Username = register.Username;
            busOperator.Gender = register.Gender;
            busOperator.Address = register.Address;
        }
        public BusOperator GetBusOperator()
        {
            return busOperator;
        }
    }
}
