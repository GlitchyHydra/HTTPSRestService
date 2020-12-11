
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    /*
    * Table name: freelancers 
    * key: id
    * fk user_id 
    * fk info_id
    */
    [Table("freelancers")]
    public class Freelancer
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("info_id")]
        public int InfoId { get; set; }
        [NotMapped]
        public List<Application> Applications { get;set; }
    }
}
