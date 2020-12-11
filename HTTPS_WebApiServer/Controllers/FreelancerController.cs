using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataLayer.Services;
using DataLayer.Models;

namespace HTTPS_WebApiServer.Controllers
{
    [Authorize(Roles = "Freelancer"), ApiController]
    public class FreelancerController : Controller
    {
        private readonly IFreelancerService freelancerService;
        public FreelancerController(IFreelancerService serv)
        {
            freelancerService = serv;
        }

        //https://glitchyhydra.ddns.net/orders/opened
        [HttpGet, Authorize, Route("orders/opened")]
        //Get all orders
        public List<Order> GetOpenedOrders()
        {
            return freelancerService.GetOpenedOrders();
        }

        //https://glitchyhydra.ddns.net/applications/
        [HttpGet, Authorize, Route("applications")]
        //Get all applications
        public List<Application> GetApplications()
        {
            return freelancerService.GetApplications();
        }

        //https://glitchyhydra.ddns.net/orders/<id?>
        [HttpPost, Authorize, Route("orders/{id?}")]
        //Add new application
        public IActionResult InsertApplicationToOrder(int? id)
        {
            if (id is null) return StatusCode(400);
            var res = freelancerService.InsertApplication((int)id);
            if (res) return StatusCode(200);
            else return StatusCode(400);
        }

        //https://glitchyhydra.ddns.net/work/<id?>
        [HttpPut, Authorize, Route("work/done/{id?}")]
        //Change status of order
        public void AlterWorkStatus(int? id)
        {
            //TODO MAKE Update Work Status
        }
    }
}
