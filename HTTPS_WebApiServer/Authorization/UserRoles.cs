using System.ComponentModel;

namespace FreelancerWeb.Authorization
{
    public enum UserRole
    {
        [Description("freelancer")]
        Freelancer       =  1,
        [Description("customer")]
        Customer         =  2,
        [Description("none")]
        None             = -1
    }

    static class UserActionManager
    {
        private static bool IsCustomerAction(UserActions action)
        {
            return (UserActions.CustomerActions & action) != 0;
        }

        private static bool IsFreelancerAction(UserActions action)
        {
            return (UserActions.FreelancerActions & action) != 0;
        }

        //return true if user has a proper role
        public static bool IsAuthorized(UserRole role, UserActions action)
        {
            return role switch
            {
                UserRole.Customer => UserActionManager.IsCustomerAction(action),
                UserRole.Freelancer => UserActionManager.IsFreelancerAction(action),
                _ => false,
            };
        }
    }

    public class WebRoles
    {
        public const string Freelancer = "Freelancer";
        public const string Customer = "Customer";
    }
}