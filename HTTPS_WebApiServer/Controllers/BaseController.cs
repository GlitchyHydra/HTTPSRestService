using FreelancerWeb.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using FreelancerWeb.Authentication;
using System.Security.Claims;
using Swashbuckle.Swagger.Annotations;
using System.Net;

namespace FreelancerWeb.Controllers
{
    [Authorize(AuthenticationSchemes = FreelancerAuthDefaults.AuthenticationScheme)]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "Invalid token")]
    public class BaseController : Controller
    {
        protected int Id { get 
            {
                var id = HttpContext.Items["Id"];
                return id is null ? -1 : (int)id;
            } }
        protected int NotAuthroized = -1;
        protected const string TokenInvalidAnswer = "Invalid token";

        protected UserRole GetRole()
        {
            UserRole role = UserRole.None;
            var roleStrNullable = HttpContext.Items[ClaimTypes.Role];
            string roleStr = roleStrNullable is null ? "" : (string)roleStrNullable;
            Enum.GetNames(typeof(UserRole)).Contains(roleStr);
            role = Enum.GetNames(typeof(UserRole)).Contains(roleStr) ? (UserRole)Enum.Parse(typeof(UserRole), roleStr, true) : UserRole.None;
            return role;
        }
    }
}
