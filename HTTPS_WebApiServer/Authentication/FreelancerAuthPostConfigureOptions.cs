using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreelancerWeb.Authentication
{
    public class FreelancerAuthPostConfigureOptions : IPostConfigureOptions<FreelancerAuthOptions>
    {
        public void PostConfigure(string name, FreelancerAuthOptions options)
        {
            if (string.IsNullOrEmpty(options.Realm))
            {
                throw new InvalidOperationException("Relam must be provided in options");
            }
        }
    }
}
