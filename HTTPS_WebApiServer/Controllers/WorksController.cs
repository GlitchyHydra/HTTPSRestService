using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMiddleware.Interfaces;
using FreelancerWeb.DataLayer.Models.Database;
using FreelancerWeb.DataLayer.Models.Server;
using FreelancerWeb.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace FreelancerWeb.Controllers
{
    /// <summary>
    /// All works from applying to order
    /// </summary>
    [Route("/works")]
    public class WorksController : BaseController
    {
        private readonly IUserDataService data_service;

        public WorksController(IUserDataService serv)
        {
            data_service = serv;
        }

        /// <summary>
        /// Update work status
        /// </summary>
        /// <param name="work_id"></param>
        /// <param name="up_status"></param>
        /// <returns></returns>
        [HttpPut("{work_id:int}")]
        [Authorize(Roles = WebRoles.Customer)]
        [Authorize(Roles = WebRoles.Freelancer)]
        public async Task<IActionResult> UpdateWorkStatus(int? work_id, [FromBody] StatusUpdateModel up_status)
        {
            if (work_id is null) return BadRequest("Lack of Application id");
            if (Id == NotAuthroized)
                return Unauthorized();
            WorkStatus status = (WorkStatus)System.Enum.Parse(typeof(WorkStatus), up_status.status, true);
            var res = await data_service.UpdateWorkStatus((int)work_id, status);
            return res ? Ok(new { Answer = "Work status updated" })
                : Ok(new { Answer = "Error in work status updating" });
        }

        /// <summary>
        /// Get all works of authorized freelancer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = WebRoles.Freelancer)]
        public async Task<IActionResult> GetWorks()
        {
            //user verification
            var freelncaer_id = Id;
            if (freelncaer_id == NotAuthroized)
                return Unauthorized("Invalid token");
            var res = await data_service.GetWorks(freelncaer_id);
            return Ok(res);
        }
    }
}
