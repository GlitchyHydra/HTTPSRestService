using System.Threading.Tasks;
using FreelancerWeb.Authorization;
using FreelancerWeb.DataLayer.Models.Server;

namespace UserMiddleware.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<int> Authenthicate(LoginModel model);
        public Task<string> Authorize(int user_id);
        public Task<UserRole> GetRoleById(int user_id);
    }
}
