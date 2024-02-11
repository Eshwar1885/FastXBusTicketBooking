using FastX.Interfaces;
using FastX.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteeController : ControllerBase
    {
        private readonly IRouteeService _adminService;


        public RouteeController(IRouteeService adminService)
        {
            _adminService = adminService;
        }
        //[HttpPost]
        //public async Task<Routee> Post(Routee Routee)
        //{
        //    var addedRoutee = await _adminService.AddRoutee(Routee);
        //    return addedRoutee;
        //}



        //[HttpGet]
        //public async Task<List<Routee>> GetAll()
        //{
        //    var routee = await _adminService.GetRouteeList();
        //    return routee;
        //}

        //[Route("/GetRouteById")]
        //[HttpGet]
        //public async Task<Routee> GetById(int id)
        //{
        //    var routee = await _adminService.GetRoutee(id);
        //    return routee;
        //}

        //[HttpDelete]
        //public async Task<Routee> Delete(int id)
        //{
        //    var routee = await _adminService.DeleteRoutee(id);
        //    return routee;
        //}

    }
}
