//using FastX.Exceptions;
//using FastX.Interfaces;
//using FastX.Models;

//namespace FastX.Services
//{
//    public class AdminService : IAdminService
//    {
//        private IAdminRepository<AllUser> _userRepository;
//        private readonly ILogger<RouteeService> _logger;

//        public AdminService(IAdminRepository<AllUser> userRepository, ILogger<RouteeService> logger)
//        {
//            _userRepository = userRepository;
//            _logger = logger;
//        }

//        public async Task DeleteUserAsync(string username)
//        {
//            var user = await _userRepository.GetByIdAsync(username);
//            if (user != null)
//            {
//                _userRepository.DeleteAsync(username);
//                await _userRepository.SaveChangesAsync();
//            }
//        }
//    }
//}
