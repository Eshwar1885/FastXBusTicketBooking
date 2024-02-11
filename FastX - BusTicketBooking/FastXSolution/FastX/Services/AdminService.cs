//using FastX.Exceptions;
//using FastX.Interfaces;
//using FastX.Models;

//namespace FastX.Services
//{
//    public class AdminService : IAdminService
//    {
//        private IRepository<int, Admin> _repo;
//        private readonly ILogger<RouteeService> _logger;

//        public AdminService(IRepository<int, Admin> repo, ILogger<RouteeService> logger)
//        {
//            _repo = repo;
//            _logger = logger;
//        }
//        public async Task<Admin> DeleteAdmin(string name)
//        {
//            var admin = await GetRoutee(id);
//            if (admin != null)
//            {
//                admin = await _repo.Delete(name);
//                return admin;
//            }
//            throw new NoSuchRouteeException();
//        }
//    }
//}
