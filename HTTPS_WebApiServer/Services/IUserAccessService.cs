using DataLayer.Models.Server;
using DataLayer.Models.DatabaseModels;
using System.Threading.Tasks;

namespace UserMiddleware.Interfaces
{
    public enum UserAccessError
    {
        Invalid = -1,
        Expired = -2,
        NotAuthorized = -3
    }

    public interface IUserAccessService
    {
        public Task<int> Authenticate(string token, UserActions action);

    }
}
