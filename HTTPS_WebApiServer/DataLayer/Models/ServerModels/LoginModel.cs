using System.ComponentModel.DataAnnotations;

namespace FreelancerWeb.DataLayer.Models.Server
{
    /// <summary>
    /// An user and a login
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// The login of user account
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        public string Login { get; set; }

        /// <summary>
        /// The password of user account
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
