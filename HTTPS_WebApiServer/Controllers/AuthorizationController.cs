using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataLayer;
using DataLayer.Services;
using DataLayer.Models;
using HTTPS_WebApiServer.Contracts;

namespace HTTPS_WebApiServer.Controllers
{
    public class LoginResponse
    {
        public string Token { get; set; }
    }

    [Route("login")]
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService authorizationService;

        public AuthorizationController(IAuthorizationService serv)
        {
            authorizationService = serv;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //TODO REDIRECT FROM HTTP TO HTTPS WHEN TRYING TO LOGIN
            var role = authorizationService.Authenthicate(model);
            if (role == RoleId.None) return Unauthorized();
            else
            {
                var token = authorizationService.Authorize(model).Result;
                return new ObjectResult(new LoginResponse { Token = token });
            }
        }
    }
}
