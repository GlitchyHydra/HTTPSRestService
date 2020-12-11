using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using HTTPS_WebApiServer.Contracts;
using System.Security.Cryptography;

namespace DataLayer.Services
{
    public enum RoleId
    {
        None = -1,
        Freelancer = 1,
        Customer = 2
    }

    public interface IAuthorizationService
    {
        public RoleId Authenthicate(LoginModel model);
        public Task<string> Authorize(LoginModel model);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly FreelancerContext context;

        public AuthorizationService(FreelancerContext ctx)
        {
            context = ctx;
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
        public RoleId Authenthicate(LoginModel model)
        {
            var user = context.Users.FirstOrDefault(u => u.Login == model.Login);
            if (user is null)
                return RoleId.None;
            else
            {
                string passInBody;
                using (MD5 md5Hash = MD5.Create())
                {
                    passInBody = GetMd5Hash(md5Hash, model.Password);
                }
                if (passInBody == user.Password)
                {
                    int id = user.Id;
                    var freelancer = context.Freelancers.FirstOrDefault(f => f.UserId == id);
                    var customer = context.Customers.FirstOrDefault(c => c.UserId == id);
                    if (freelancer != null) return RoleId.Freelancer;
                    else if (customer != null) return RoleId.Customer;
                }
                return RoleId.None;
            }
        }

        //User try to acquire a new token for session
        public async Task<string> Authorize(LoginModel model)
        {
            string passHash;
            //String.Join("", System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.ASCII.GetBytes(model.Password)).Select(Function(x) x.ToString("x2")));
            using (MD5 md5Hash = MD5.Create())
            {
                passHash = GetMd5Hash(md5Hash, model.Password);
            }
            return passHash;
        }
    }
}
