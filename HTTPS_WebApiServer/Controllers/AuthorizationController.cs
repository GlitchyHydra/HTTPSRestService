using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Models.Server;
using UserMiddleware.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HTTPS_WebApiServer.Controllers
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
        //[ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
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
                    string role = "";
                    var role_ans = await authorization_service.GetRoleById(user_id);
                    if (role_ans == UserRole.Customer)
                        role = "customer";
                    else if (role_ans == UserRole.Freelancer)
                        role = "freelancer";
                    else
                        role = "none";
                    return Ok(new { Token = token, Role = role});
                }
            }
        }
    }
}
