using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMiddleware.Interfaces;
using DataLayer.Models.Database;
using DataLayer.Models.Server;
using DataLayer.Models.DatabaseModels;
using System;

namespace HTTPS_WebApiServer.Controllers
{
    /// <summary>
    /// All works from applying to order
    /// </summary>
    [Route("/works")]
    public class WorksController : Controller
    {
        private readonly IUserDataService data_service;
        private readonly IUserAccessService user_access_service;

        public WorksController(IUserDataService serv, IUserAccessService uas)
        {
            data_service = serv;
            user_access_service = uas;
        }

        /// <summary>
        /// Status of work
        /// </summary>
        [Serializable]
        public class UpWorkStatus
        {
            /// <summary>
            /// Status
            /// </summary>
            /// <example>"processing", "done" or "close"</example>
            /// <remarks>
            /// "processing" work in progress
            /// "done" freelancer have complete work and wait for accepting by customer
            /// "close" accepted by customer
            /// </remarks>
            public string status { get; set; }
        };

        /// <summary>
        /// Update work status
        /// </summary>
        /// <param name="work_id"></param>
        /// <param name="up_status"></param>
        /// <returns></returns>
        [HttpPut("{work_id:int}")]
        public async Task<IActionResult> UpdateWorkStatus(int? work_id, [FromBody] UpWorkStatus up_status)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            if (work_id is null) return BadRequest("Lack of Application id");
            else if (!find_token_res) return Unauthorized("Lack of token");
            else
            {
                //TODO if status
                var authResult = await user_access_service.Authenticate(token, UserActions.UpdateWorkStatus);
                if (authResult > -1)
                {
                    WorkStatus status = (WorkStatus)System.Enum.Parse(typeof(WorkStatus), up_status.status, true);
                    var res = await data_service.UpdateWorkStatus((int)work_id, status);
                    return res ? Ok(new { Answer = "Work status updated" }) : Ok(new { Answer = "Error in work status updating" });
                }
                else return Unauthorized();
            }
        }

        /// <summary>
        /// Get all works of authorized freelancer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetWorks()
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            //check if user data present
            if (!find_token_res) return Unauthorized("Lack of token");
            //user verification
            var freelncaer_id = await user_access_service.Authenticate(token, UserActions.GetWorks);
            if (freelncaer_id > -1)
            {
                var res = await data_service.GetWorks(freelncaer_id);
                return Ok(res);
            }
            else return Unauthorized("Invalid token");
        }
    }
}
