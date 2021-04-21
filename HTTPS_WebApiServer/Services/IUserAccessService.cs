using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

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
        public Task<IList<Claim>> Authenticate(string token);

    }
}
