using DataLayer.Models.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using UserMiddleware.Interfaces;
using DataLayer;
using DataLayer.Models.DatabaseModels;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace UserMiddleware.Services
{
    static class UserActionManager
    {
        public static bool IsCustomerAction(UserActions action)
        {
            return (UserActions.CustomerActions & action) != 0;
        }

        public static bool IsFreelancerAction(UserActions action)
        {
            return (UserActions.FreelancerActions & action) != 0;
        }
    }

    public class UserAccessService : IUserAccessService
    {
        
        private readonly FreelancerContext context;

        /*
         * { UserRole.Customer, new HashSet<string>() { "GetOpenedOrders", "GetOwnedApplications", "ApplyToOrder", "DoneWork" } },
            { UserRole.Freelancer, new HashSet<string>() {"InsertOrder", "GetOwnedOrders", "AlterOrderStatus"} }
         */

        public UserAccessService(FreelancerContext ctx)
        {
            context = ctx;
        }

        //return true if user has a proper role
        private bool IsAuthorized(UserRole role, UserActions action)
        {
            return role switch
            {
                UserRole.Customer   => UserActionManager.IsCustomerAction(action),
                UserRole.Freelancer => UserActionManager.IsFreelancerAction(action),
                _                   => false,
            };
        }

        private bool IsTokenExpired(string token)
        {
            var token_claims = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = token_claims.Claims.First(c => c.Type == "exp").Value;
            long unixDate = long.Parse(claim);
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime exp_date = start.AddMilliseconds(unixDate);
            var delta_time = DateTime.Now.Subtract(exp_date);
            double delta_in_days = delta_time.TotalDays;
            return delta_in_days < 7;
        }

        private int GetIdByToken(string token)
        {
            var token_claims = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = token_claims.Claims.First(c => c.Type == "Id").Value;
            return int.Parse(claim);
        }

        private UserRole GetRoleByToken(string token)
        {
            var token_claims = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = token_claims.Claims.First(c => c.Type == "Role").Value;
            return Enum.GetNames(typeof(UserRole)).Contains(claim) ? (UserRole)Enum.Parse(typeof(UserRole), claim, true) : UserRole.None;
        }

        //return user id if success or -1
        public async Task<int> Authenticate(string token, UserActions action)
        {
            if (IsTokenExpired(token))
                return (int)UserAccessError.Expired;

            var role = GetRoleByToken(token);
            var isAuthorizedTo = IsAuthorized(role, action);
            if (!isAuthorizedTo)
                return (int)UserAccessError.NotAuthorized;

            var user_id = GetIdByToken(token);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == user_id);
            if (user is null) return (int)UserAccessError.Invalid;
            else
            {
                if (user.Token != token)
                    return (int)UserAccessError.Invalid;

                switch (role)
                {
                    case UserRole.Customer:
                        {
                            var res = await context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
                            return res.Id;
                        }
                    case UserRole.Freelancer:
                        {
                            var res = await context.Freelancers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                            return res.Id;
                        }
                }
                return (int)UserAccessError.Invalid;
            }
        }
    }
}
