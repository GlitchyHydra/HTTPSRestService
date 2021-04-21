using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using UserMiddleware.Interfaces;

namespace FreelancerWeb.Authentication
{
    public class FreelancerAuthScheme : AuthenticationScheme
    {
        public FreelancerAuthScheme(string name, string displayName, Type handlerType)
            : base(name, displayName, handlerType)
        {

        }
    }

    public class FreelancerAuthHandler : AuthenticationHandler<FreelancerAuthOptions>
    {
        private const string AuthorizationField = "Authorization";
        private readonly IUserAccessService userAccessService;

        public FreelancerAuthHandler(IOptionsMonitor<FreelancerAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserAccessService user_access_service)
            : base(options, logger, encoder, clock)
        {
            userAccessService = user_access_service;
        }

        protected override string ClaimsIssuer => base.ClaimsIssuer;

        protected override object Events { get => base.Events; set => base.Events = value; }

        protected override Task<object> CreateEventsAsync()
        {
            return base.CreateEventsAsync();
        }

        /// <summary>
        /// Searches the 'Authorization' header for a 'Bearer' token. If the 'Bearer' token is found, it is validated using <see cref="TokenValidationParameters"/> set in the options.
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(AuthorizationField))
            {
                return AuthenticateResult.NoResult();
            }

            var rawHead = Request.Headers[AuthorizationField];

            var claims = await userAccessService.Authenticate(rawHead);
            int id = 0;
            string role = string.Empty;

            foreach(Claim claim in claims)
            {
                if (claim.Type == "Id")
                    id = int.Parse(claim.Value);
                else if (claim.Type == ClaimTypes.Role)
                    role = claim.Value;
            }
            if (id <= 0)
                return AuthenticateResult.Fail("");

            var TokenId = "Id";
            Context.Items[TokenId] = id;
            Context.Items[ClaimTypes.Role] = role;
            
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }


        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Options.Realm}\", charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return base.HandleForbiddenAsync(properties);
        }

        protected override Task InitializeEventsAsync()
        {
            return base.InitializeEventsAsync();
        }

        protected override Task InitializeHandlerAsync()
        {
            return base.InitializeHandlerAsync();
        }

        protected override string ResolveTarget(string scheme)
        {
            return base.ResolveTarget(scheme);
        }
    }
}
