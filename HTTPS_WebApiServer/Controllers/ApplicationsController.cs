using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMiddleware.Interfaces;
using FreelancerWeb.DataLayer.Models.Database;
using FreelancerWeb.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using FreelancerWeb.DataLayer.Models.Server;

namespace FreelancerWeb.Controllers
{
    [Route("/applications")]
    public class ApplicationsController : BaseController
    {
        private readonly IUserDataService data_service;

        public ApplicationsController(IUserDataService serv)
        {
            data_service = serv;
        }

        /*------------------------------------------Only Customer----------------------------------------------*/

        /// <summary>
        /// Accept application by the customer
        /// </summary>
        /// <param name="application_id">Application id for updating</param>
        /// <returns></returns>
        /// <response code="200">Updated successfully</response>
        /// <response code="500">Error in inserting</response>
        [HttpPut("{application_id:int}")]
        [Authorize(Roles = WebRoles.Customer)]
        public async Task<IActionResult> AcceptApplication(int application_id)
        {
            int customerId = Id;
            if (customerId == NotAuthroized)
                return Unauthorized(new FreelancerResponse { Message = TokenInvalidAnswer });
            var res = await data_service.UpdateApplicationStatus((int)application_id, ApplcationStatus.Accepted);
            if (res) return Ok(new FreelancerResponse{ Message = "Application to order was accepted" });
            else     return StatusCode(StatusCodes.Status500InternalServerError,
                new FreelancerResponse() { Message = "Error in work inserting" });
        }

        /*------------------------------------------Only Freelancer----------------------------------------------*/
       
        /// <summary>
        /// Get all applications owned by the authorized freelancer 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Applications by the freelancer</response>
        [HttpGet]
        [Authorize(Roles = WebRoles.Freelancer)]
        public async Task<IActionResult> GetApplications()
        {
            var freelancer_id = Id;
            if (freelancer_id == NotAuthroized) 
                return Unauthorized(new FreelancerResponse { Message = TokenInvalidAnswer });
            var applications = await data_service.GetApplications(freelancer_id);
            return Ok(applications);
        }

    }
}
