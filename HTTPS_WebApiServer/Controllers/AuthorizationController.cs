using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Models.Server;
using UserMiddleware.Interfaces;

namespace HTTPS_WebApiServer.Controllers
{
    [Route("/login")]
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService authorization_service;

        public AuthorizationController(IAuthorizationService serv)
        {
            authorization_service = serv;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model is null) return BadRequest(new { errorText = "lack of username or password" });
            else
            {
                //TODO REDIRECT TO HTTPS WHEN TRY TO LOGIN
                var user_id = await authorization_service.Authenthicate(model);
                if (user_id == -1) return Unauthorized(new { errorText = "Invalid username or password" });
                else
                {
                    var token = await authorization_service.Authorize(user_id);
                    return Ok(token);
                }
            }
        }
    }
}
