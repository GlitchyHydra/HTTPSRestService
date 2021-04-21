using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FreelancerWeb.DataLayer.Models.Database
{
    
    public enum OrderStatus
    {
        Open       = 0,
        Processing = 1,
        Close      = 2
    }

    /// <summary>
    /// Order 
    /// </summary>
    [Table("orders")]
    public class Order
    {
        /// <summary>
        /// Id
        /// </summary>
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// Id of customer who created the order
        /// </summary>
        [Column("customer_id")]
        public int CustomerId { get; set; }
        
        /// <summary>
        /// Order of creating
        /// </summary>
        [Column("order_date")]
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [Column("status")]
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Column("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// Applications to the order
        /// </summary>
        public List<Application> Applications { get; set; }
    }
}
