using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreelancerWeb.Authentication
{
    public class MessageReceivedContext : ResultContext<FreelancerAuthOptions>
    {
        public MessageReceivedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            FreelancerAuthOptions options)
            : base(context, scheme, options) { }

        /// <summary>
        /// Bearer Token. This will give the application an opportunity to retrieve a token from an alternative location.
        /// </summary>
        public string Token { get; set; }
    }
}
