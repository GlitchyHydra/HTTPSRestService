using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models.Server;

namespace UserMiddleware.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<int> Authenthicate(LoginModel model);
        public Task<string> Authorize(int user_id);
        public Task<UserRole?> GetRoleById(int user_id);
    }
}
