using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FreelancerWeb.DataLayer.Models.Server;
using UserMiddleware.Interfaces;
using FreelancerWeb.Authorization;

namespace FreelancerWeb.Controllers
{
    /// <summary>
    /// login for token
    /// </summary>
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService authorization_service;

        public AuthorizationController(IAuthorizationService serv)
        {
            authorization_service = serv;
        }

        /// <summary>
        /// This method is using for first logging when user does not has token
        /// </summary>
        /// <remarks>
        /// Example:
        ///     POST /login
        ///     {
        ///     "login": "mylogin"
        ///     "password": "mypassword"
        ///     }
        /// </remarks>
        /// <param name="model">User and password</param>
        /// <returns>token</returns>
        /// <response code="200">Returns newly created token</response>
        /// <response code="401">Error in login or password</response>
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model is null) return Unauthorized(new { errorText = "Invalid username or password" });
            var user_id = await authorization_service.Authenthicate(model);
            if (user_id == -1) return Unauthorized(new { errorText = "Invalid username or password" });
            else
            {
                var token = await authorization_service.Authorize(user_id);
                string role = "";
                var role_ans = await authorization_service.GetRoleById(user_id);
                switch (role_ans)
                {
                    case UserRole.Customer: role = "customer"; break;
                    case UserRole.Freelancer: role = "freelancer"; break;
                    default: role = "none"; break;
                }
                return Ok(new { Token = token, Role = role });

            }
        }
    }
}
