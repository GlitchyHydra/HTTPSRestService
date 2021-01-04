using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.Database
{
    /*
    * Table name: users
    * key: id
    * string[64] login
    * string[64] password
    */

    [Table("users")]
    public class User 
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("login")]
        public string Login { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("token")]
        public string Token { get; set; }
    }
}
