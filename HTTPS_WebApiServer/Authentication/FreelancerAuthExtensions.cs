using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using UserMiddleware.Interfaces;

namespace FreelancerWeb.Authentication
{
    public static class FreelancerAuthExtensions
    {
        public static AuthenticationBuilder AddFreelancerAuth<TAuthService>(this AuthenticationBuilder builder)
            where TAuthService : class, IUserAccessService
        {
            return AddFreelancerAuth<TAuthService>(builder, FreelancerAuthDefaults.AuthenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddFreelancerAuth<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme)
            where TAuthService : class, IUserAccessService
        {
            return AddFreelancerAuth<TAuthService>(builder, authenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddFreelancerAuth<TAuthService>(this AuthenticationBuilder builder, Action<FreelancerAuthOptions> configureOptions)
            where TAuthService : class, IUserAccessService
        {
            return AddFreelancerAuth<TAuthService>(builder, FreelancerAuthDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddFreelancerAuth<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme, Action<FreelancerAuthOptions> configureOptions)
            where TAuthService : class, IUserAccessService
        {
            builder.Services.AddSingleton<IPostConfigureOptions<FreelancerAuthOptions>, FreelancerAuthPostConfigureOptions>();
            builder.Services.AddTransient<IUserAccessService, TAuthService>();

            return builder.AddScheme<FreelancerAuthOptions, FreelancerAuthHandler>(authenticationScheme, configureOptions);
        }
    }
}
