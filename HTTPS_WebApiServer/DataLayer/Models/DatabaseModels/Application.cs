using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.Database
{
    public enum ApplcationStatus
    {
        Open     = 0,
        Accepted = 1,
        Rejected = 2
    }

    [Table("applications")]
    public class Application
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("freelancer_id")]
        public int FreelancerId { get; set; }
        [Column("apply_date")]
        public DateTime ApplyDate { get; set; }
        [Column("status")]
        public ApplcationStatus Status { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
    }
}
