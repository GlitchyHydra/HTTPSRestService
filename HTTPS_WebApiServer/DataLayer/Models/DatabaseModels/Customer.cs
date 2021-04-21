using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerWeb.DataLayer.Models.Database
{
    [Table("customers")]
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
