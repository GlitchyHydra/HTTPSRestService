using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.Database
{
    public class Customer
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get;set; }
        [Column("info_id")]
        public int InfoId { get; set; }
    }
}
