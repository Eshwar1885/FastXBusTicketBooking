using FastX.Models;

namespace FastX.Interfaces
{
    public interface IRouteeService
    {
        public Task<Routee> AddRoutee(Routee routee);

        public Task<List<Routee>> GetRouteeList();
        public Task<Routee> GetRoutee(int id);

        public Task<Routee> DeleteRoutee(int id);


    }
}