using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using UserMiddleware.Interfaces;
using FreelancerWeb.DataLayer.Models.Server;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using FreelancerWeb.Authorization;

namespace DataLayer.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly FreelancerContext context;

        public AuthorizationService(FreelancerContext ctx)
        {
            context = ctx;
        }

        public class AuthOptions
        {
            public const string ISSUER   = "FreelancerAuthServer";  // издатель токена
            public const string AUDIENCE = "Freelancers";           // потребитель токена
            const string KEY             = "SPBSTU_secretkey";      // Cipher key
            public const int LIFETIME    = 7;                       // token TTL (in days)
            public static SymmetricSecurityKey GetSymmetricSecurityKey()
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
            }
        }

        //md5 hash of pass
        private string GetMd5Hash(MD5 hash, string pass)
        {
            byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(pass));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        //Identify and authenticate a user
        public async Task<int> Authenthicate(LoginModel model)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
            if (user is null) return -1;
            else
            {
                string passInBody;
                using (MD5 md5Hash = MD5.Create())
                {
                    passInBody = GetMd5Hash(md5Hash, model.Password);
                }
                return (passInBody == user.Password) ? user.Id : -1;
            }
        }

        public async Task<UserRole> GetRoleById(int user_id)
        {
            var freelancer = await context.Freelancers.FirstOrDefaultAsync(f => f.UserId == user_id);
            if (freelancer != null) return UserRole.Freelancer;

            var customer = await context.Customers.FirstOrDefaultAsync(c => c.UserId == user_id);
            if (customer != null) return UserRole.Customer;

            return UserRole.None;
        }

        private async Task<string> GenerateJwtToken(int user_id)
        {
            var role            = await GetRoleById(user_id); 
            var tokenHandler    = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer   = AuthOptions.ISSUER,
                Audience = AuthOptions.AUDIENCE,
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim("Id", user_id.ToString()),
                    new Claim("Role", role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(AuthOptions.LIFETIME),
                SigningCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> WriteToken(int user_id, string token)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == user_id);
            user.Token = token;
            var res = await context.SaveChangesAsync();
            return res != 0;
        }

        //User try to acquire a new token for session
        public async Task<string> Authorize(int user_id)
        {
            string token = await GenerateJwtToken(user_id);
            var res = await WriteToken(user_id, token);
            return res ? token : "BadToken"; 
        }
    }
}
