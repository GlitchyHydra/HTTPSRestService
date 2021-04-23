using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMiddleware.Interfaces;
using FreelancerWeb.DataLayer.Models.Database;
using FreelancerWeb.DataLayer.Models.Server;
using FreelancerWeb.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

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
        /// <response code="200">status of work was updated</response>
        /// <response code="400">need to specify a work_id</response>
        /// <response code="500">error on updating</response>
        [HttpPut("{work_id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateWorkStatus(int? work_id, [FromBody] StatusUpdateModel up_status)
        {
            if (work_id is null) return BadRequest(new FreelancerResponse { Message = "Lack of Application id" });
            if (Id == NotAuthroized) return Unauthorized();
            WorkStatus status = (WorkStatus)System.Enum.Parse(typeof(WorkStatus), up_status.status, true);
            var res = await data_service.UpdateWorkStatus((int)work_id, status);
            if (res) return Ok(new FreelancerResponse{ Message = "Work status updated" });
            else     return StatusCode(StatusCodes.Status500InternalServerError,
                new FreelancerResponse() { Message = "Error on updating" });
        }

        /// <summary>
        /// Get all works of authorized freelancer
        /// </summary>
        /// <returns></returns>
        /// <response code="200">works owned by authorized freelancer</response>
        [HttpGet]
        [Authorize(Roles = WebRoles.Freelancer)]
        public async Task<IActionResult> GetWorks()
        {
            var freelncaer_id = Id;
            if (freelncaer_id == NotAuthroized)
                return Unauthorized(new FreelancerResponse { Message = TokenInvalidAnswer });
            var res = await data_service.GetWorks(freelncaer_id);
            return Ok(res);
        }
    }
}
