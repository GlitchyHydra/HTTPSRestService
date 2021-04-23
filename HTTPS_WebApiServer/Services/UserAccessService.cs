using System;
using System.Collections.Generic;
using System.Linq;
using UserMiddleware.Interfaces;
using DataLayer;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FreelancerWeb.Authorization;

namespace UserMiddleware.Services
{
    public class UserAccessService : IUserAccessService
    {
        private readonly FreelancerContext context;

        public UserAccessService(FreelancerContext ctx)
        {
            context = ctx;
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
        public async Task<IList<Claim>> Authenticate(string token)
        {
            IList<Claim> claims = new List<Claim>();

            bool canBeRead = new JwtSecurityTokenHandler().CanReadToken(token);
            if (!canBeRead)
                return claims;

            if (IsTokenExpired(token))
            {
                claims.Add(new Claim(ClaimTypes.Expired, "Expired"));
                claims.Add(new Claim(ClaimTypes.Authentication, "Invalid"));
                return claims;
            }
                
            var role = GetRoleByToken(token);
            var user_id = GetIdByToken(token);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == user_id);
            if (user is null || user.Token != token)
            {
                claims.Add(new Claim(ClaimTypes.Expired, "Expired"));
                claims.Add(new Claim(ClaimTypes.Authentication, "Invalid"));
                return claims;
            }

            int id = 0;
            switch (role)
            {
                case UserRole.Customer:
                    {
                        var res = await context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
                        id = res.Id;
                        claims.Add(new Claim(ClaimTypes.Role, UserRole.Customer.ToString()));
                        break;
                    }
                case UserRole.Freelancer:
                    {
                        var res = await context.Freelancers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                        id = res.Id;
                        claims.Add(new Claim(ClaimTypes.Role, UserRole.Freelancer.ToString()));
                        break;
                    }
            }
            claims.Add(new Claim("Id", id.ToString()));
            return claims;

        }
    }
}
