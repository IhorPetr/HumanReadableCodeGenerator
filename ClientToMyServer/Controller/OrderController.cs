using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClientToMyServer.Services.Storages;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientToMyServer.Controllers
{
    public class OrderController : Controller
    {
        private IOrderList _list;
        public OrderController(IOrderList _list)
        {
            this._list = _list;
        }
        // GET: api/values
        [HttpGet]
        [Route("Change")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _list.GetAll());
        }
    }
}
