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
        /*
         * glitchyhydra.ddns.net/applications/ 
         * return result of application status updating 
         * by customer
         */
        [HttpPut, Route("/{application_id?}")]
        public async Task<IActionResult> AcceptApplication(int? application_id)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            if (application_id is null) return BadRequest("Lack of Application id");
            else if (!find_token_res) return Unauthorized("Lack of token");
            else
            {
                var authResult = await user_access_service.Authenticate(token, UserActions.AcceptApplication);
                if (authResult > -1)
                {
                    var res = await data_service.UpdateApplicationStatus((int)application_id, ApplcationStatus.Accepted);
                    return res ? Ok("Application to order was accepted") : Ok("Error in application accepting");
                }
                else return Unauthorized();
            }
        }

        /*------------------------------------------Only Freelancer----------------------------------------------*/
        /*
         * glitchyhydra.ddns.net/applications/ 
         * return owned by 'freelancer id' applications
         */
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
