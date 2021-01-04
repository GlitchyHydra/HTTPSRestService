using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMiddleware.Interfaces;
using DataLayer.Models.Database;
using DataLayer.Models.Server;
using DataLayer.Models.DatabaseModels;

namespace HTTPS_WebApiServer.Controllers
{
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

        [HttpPut, Route("/{work_id?}")]
        public async Task<IActionResult> UpdateWorkStatus(int? work_id, [FromBody] WorkStatus status)
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
                    var res = await data_service.UpdateWorkStatus((int)work_id, status);
                    return res ? Ok("Application to order was accepted") : Ok("Error in work status updating");
                }
                else return Unauthorized();
            }
        }

        /*
         * glitchyhydra.ddns.net/works/ 
         * return assigned to freelancer works
         */
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
