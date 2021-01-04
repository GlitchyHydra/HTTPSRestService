using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.Database
{
    public enum WorkStatus
    {
        Open        = 0,
        Processing  = 1,
        Done        = 2,
        Close       = 3
    }

    [Table("works")]
    public class Work
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("freelancer_id")]
        public int FreelancerId { get; set; }
        [Column("work_date")]
        public DateTime WorkDate { get; set; }
        [Column("status")]
        public WorkStatus Status { get; set; }
    }
}
