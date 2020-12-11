using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataLayer.Services;
using DataLayer.Models;
using HTTPS_WebApiServer.Contracts;

namespace HTTPS_WebApiServer.Controllers
{
    //[Authorize(Roles = "Customer"), ApiController]
    public class HomeController : Controller
    {
        private readonly ICustomerService customerService;

        public HomeController(ICustomerService serv)
        {
            customerService = serv;
        }

        [HttpPost, Route("orders")]
        public async Task<IActionResult> InsertOrder([FromBody]OrderModel model)
        {
            var res = customerService.InsertOrder(model.name, model.desc);
            if (res.Result) return StatusCode(200);
            else return StatusCode(400);
        }

        [HttpGet, Route("orders")]
        //Get all orders by customer id
        public List<Order> GetAllOrders()
        {
            var orders = customerService.GetOrders().Result;
            return orders;
        }

        [HttpPut, Route("orders/{id?}")]
        //Change status of order
        public IActionResult AlterOrderStatus(int? id, [FromBody] OrderStatus status)
        {
            if (id is null) return StatusCode(400);
            else
            {
                var res = customerService.AlterOrderStatus((int)id, status);
                if (res.Result) return StatusCode(200);
                else return StatusCode(400);
            }
        }

        [HttpPost, Route("orders/{order_id?}/applications/{application_id?}")]
        //Accept one application and delete others
        public void AcceptApplicationToOrder(int? order_id, int? application_id)
        {

        }

        [HttpPut, Route("orders/{order_id?}/applications/{application_id?}")]
        //Accept one application and delete others
        public void AcceptCompletedWork(int? order_id, int? application_id)
        {

        }
    }
}
