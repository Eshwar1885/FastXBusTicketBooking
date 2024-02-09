﻿using FastX.Interfaces;
using FastX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusController : Controller
    {
        private readonly IBusService _adminService;
        public BusController(IBusService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = "busoperator")]
        [HttpPost]
        public async Task<Bus> Post(Bus Bus)
        {
            var addedBus = await _adminService.AddBus(Bus);
            return addedBus;
        }

        [HttpGet]
        public async Task<List<Bus>> GetAll()
        {
            var Bus = await _adminService.GetBusList();
            return Bus;
        }

        [Route("/GetBusById")]
        [HttpGet]
        public async Task<Bus> GetById(int id)
        {
            var Bus = await _adminService.GetBus(id);
            return Bus;
        }
        //[Authorize(Roles = "admin")]
        [Authorize(Roles = "busoperator")]
        [HttpDelete]
        public async Task<Bus> Delete(int id)
        {
            var Bus = await _adminService.DeleteBus(id);
            return Bus;
        }


    }
}
