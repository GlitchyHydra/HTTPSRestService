using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMiddleware.Interfaces;
using DataLayer.Models.Database;
using DataLayer.Models.DatabaseModels;
using DataLayer.Models.Server;

namespace HTTPS_WebApiServer.Controllers
{
    [Route("/applications")]
    public class ApplicationsController : Controller
    {
        private readonly IUserDataService data_service;
        private readonly IUserAccessService user_access_service;

        public ApplicationsController(IUserDataService serv, IUserAccessService uas)
        {
            data_service = serv;
            user_access_service  = uas;
        }

        /*------------------------------------------Only Customer----------------------------------------------*/

        /// <summary>
        /// Accept application by the customer
        /// </summary>
        /// <param name="application_id">Application id for updating</param>
        /// <returns></returns>
        /// <response code="200">result of updating</response>
        /// <response code="400">Lack of token in header</response>
        [HttpPut("{application_id:int}")]
        public async Task<IActionResult> AcceptApplication(int application_id)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            if (!find_token_res) return Unauthorized("Lack of token");
            else
            {
                var authResult = await user_access_service.Authenticate(token, UserActions.AcceptApplication);
                if (authResult > -1)
                {
                    var res = await data_service.UpdateApplicationStatus((int)application_id, ApplcationStatus.Accepted);
                    return res ? Ok(new { Answer = "Application to order was accepted" }) : Ok(new { Answer = "Error in work status updating" });
                }
                else return Unauthorized();
            }
        }

        /*------------------------------------------Only Freelancer----------------------------------------------*/
       
        /// <summary>
        /// Get all applications owned by the authorized freelancer 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Applications by the freelancer</response>
        /// <response code="400">Lack of token in header</response>
        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            if (!find_token_res) return Unauthorized("Lack of token");
            else
            {
                var user_id = await user_access_service.Authenticate(token, UserActions.GetApplications);
                if (user_id > -1)
                {
                    var applications = await data_service.GetApplications(user_id);
                    return Ok(applications);
                }
                else return Unauthorized("Invalid token");
            }
        }

    }
}
